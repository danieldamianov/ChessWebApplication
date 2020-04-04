﻿namespace ChessGameLogic
{
    using System;
    using System.Collections.Generic;

    using ChessGameLogic.ChessFigures;
    using ChessGameLogic.ChessFigures.Interfaces;
    using ChessGameLogic.ChessMoves;
    using ChessGameLogic.ClientInteractionEntities;
    using ChessGameLogic.Enums;

    public class ChessGame
    {
        private readonly Func<ChessFigureProductionType> chooseFigureToProduceFunction;
        private readonly Action<EndGameResult> endGameHandleFunction;

        private ChessGameProgressInfo progressInfo;
        private ChessBoard chessBoard;
        private ChessColors playerOnTurn;

        public ChessGame(Func<ChessFigureProductionType> chooseFigureToProduceFunction, Action<EndGameResult> endGameHandleFunction)
        {
            if (chooseFigureToProduceFunction == null)
            {
                throw new ArgumentNullException(nameof(chooseFigureToProduceFunction));
            }

            if (endGameHandleFunction == null)
            {
                throw new ArgumentNullException(nameof(endGameHandleFunction));
            }

            this.playerOnTurn = ChessColors.White;
            this.chessBoard = new ChessBoard();
            this.progressInfo = ChessGameProgressInfo.InProgressWithoutCheck;

            this.chooseFigureToProduceFunction = chooseFigureToProduceFunction;
            this.endGameHandleFunction = endGameHandleFunction;
        }

        public ChessColors PlayerOnTurn => this.playerOnTurn;

        public ChessGameProgressInfo GameProgressInfo => this.progressInfo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialPositionHorizontal"></param>
        /// <param name="initialPositionVertical"></param>
        /// <param name="targetPositionHorizontal"></param>
        /// <param name="targetPositionVertical"></param>
        /// <param name="figureType"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public NormalChessMoveValidationResult NormalMove(
            char initialPositionHorizontal,
            int initialPositionVertical,
            char targetPositionHorizontal,
            int targetPositionVertical,
            ChessFigureType figureType,
            ChessColors color)
        {
            if (this.progressInfo == ChessGameProgressInfo.WhiteHaveWon
                || this.progressInfo == ChessGameProgressInfo.BlackHaveWon
                || this.progressInfo == ChessGameProgressInfo.GameHasEndedDraw)
            {
                return NormalChessMoveValidationResult.GameHasEnded;
            }

            NormalChessMovePositions move = new NormalChessMovePositions(
                initialPositionHorizontal,
                initialPositionVertical,
                targetPositionHorizontal,
                targetPositionVertical);

            IFigure figure = this.chessBoard.GetFigureOnPosition(move.InitialPosition);

            Type actualFigureType = this.ConvertFromFigureTypeEnumToActualType(figureType);

            NormalChessMoveValidationResult validationResult = this.ValidateMove(move, actualFigureType, color);

            if (validationResult == NormalChessMoveValidationResult.ValidMove)
            {
                this.chessBoard.RemoveFigureOnPosition(move.InitialPosition);
                this.chessBoard.PutFigureOnPosition(move.TargetPosition, figure);

                if (figure is Pawn && ((Pawn)figure).isPositionProducable(move.TargetPosition))
                {
                    ChessFigureProductionType chessFigureProductionType = this.chooseFigureToProduceFunction();

                    Type figureToProduceType = this.ConvertFromChessFigureProductionTypeEnumToActualType(chessFigureProductionType);

                    this.ProducePawn(move.TargetPosition, figureToProduceType, color);
                }

                if (this.CheckForCheck(this.chessBoard, ChessColors.White))
                {
                    this.progressInfo = ChessGameProgressInfo.WhiteAreUnderCheck;
                }
                else if (this.CheckForCheck(this.chessBoard, ChessColors.Black))
                {
                    this.progressInfo = ChessGameProgressInfo.BlackAreUnderCheck;
                }
                else
                {
                    this.progressInfo = ChessGameProgressInfo.InProgressWithoutCheck;
                }

                if (this.CheckForDraw())
                {
                    this.endGameHandleFunction(EndGameResult.Draw);
                    this.progressInfo = ChessGameProgressInfo.GameHasEndedDraw;
                }

                if (this.CheckForMate(ChessColors.Black))
                {
                    this.endGameHandleFunction(EndGameResult.BlackWin);
                    this.progressInfo = ChessGameProgressInfo.BlackHaveWon;
                }

                if (this.CheckForMate(ChessColors.White))
                {
                    this.endGameHandleFunction(EndGameResult.WhiteWin);
                    this.progressInfo = ChessGameProgressInfo.WhiteHaveWon;
                }

                this.ChangePlayer();
            }

            return validationResult;
        }

        /// <summary>
        /// Method that gets all possible positions of placing the given figure.
        /// </summary>
        /// <param name="horizontal">The horizontal dimension of the chess board - letter from 'a' to 'h'.</param>
        /// <param name="vertical">The vertical dimension of the chess board - number from 1 to 8.</param>
        /// <param name="figureType">The type of the figure to move.</param>
        /// <param name="chessFigureColor">The color of the figure to move.</param>
        /// <returns>List of all possible positions.</returns>
        public List<Position> GetAllPossiblePositionsOfPlacingTheFigure(
            char horizontal,
            int vertical,
            ChessFigureType figureType,
            ChessColors chessFigureColor)
        {
            Type actualFigureType = this.ConvertFromFigureTypeEnumToActualType(figureType);

            ChessBoardPosition chessBoardPosition = new ChessBoardPosition(horizontal, vertical);

            List<Position> attackingPos = new List<Position>();

            for (char horizontalIterator = 'a'; horizontalIterator <= 'h'; horizontalIterator++)
            {
                for (int verticalIterator = 1; verticalIterator <= 8; verticalIterator++)
                {
                    NormalChessMovePositions move = new NormalChessMovePositions(
                        chessBoardPosition.Horizontal,
                        chessBoardPosition.Vertical,
                        horizontalIterator,
                        verticalIterator);

                    NormalChessMoveValidationResult validationResult = this.ValidateMove(
                        move,
                        actualFigureType,
                        chessFigureColor);

                    if (validationResult == NormalChessMoveValidationResult.ValidMove)
                    {
                        attackingPos.Add(new Position(horizontalIterator, verticalIterator));
                    }
                }
            }

            return attackingPos;
        }

        private ChessColors GetOppositeColor(ChessColors color)
        {
            if (color == ChessColors.Black)
            {
                return ChessColors.White;
            }
            return ChessColors.Black;
        }

        private void ChangePlayer()
        {
            this.playerOnTurn = this.GetOppositeColor(this.playerOnTurn);
        }

        private NormalChessMoveValidationResult ValidateMove(NormalChessMovePositions positions, Type FigureType, ChessColors color)
        {
            IFigure figureToMove = this.chessBoard.GetFigureOnPosition(positions.InitialPosition);
            Type actualFigureType = figureToMove.GetType();
            IFigure figureOnTargetPosition = this.chessBoard.GetFigureOnPosition(positions.TargetPosition);
            if (figureToMove == null || actualFigureType.FullName != FigureType.FullName || figureToMove.Color != color)
            {
                return NormalChessMoveValidationResult.ThereIsntSuchFigureAndColorOnTheGivenPosition;
            }
            if (figureOnTargetPosition != null &&
                (figureOnTargetPosition.GetType() == typeof(King) || figureOnTargetPosition.Color == figureToMove.Color))
            {
                return NormalChessMoveValidationResult.TheFigureOnTheTargetPositionIsFriendlyOrEnemyKing;
            }

            if (figureToMove is Pawn)
            {

                if (((Pawn)figureToMove).AreMovePositionsPossible(positions) == true
                    && figureOnTargetPosition != null)
                {
                    return NormalChessMoveValidationResult.MovePositionsAreNotValid;
                }

                if (
                        ((((Pawn)figureToMove).AreMovePositionsPossible(positions) == false &&
                    ((Pawn)figureToMove).IsAttackingMovePossible(positions) == false) ||
                            (
                                ((Pawn)figureToMove).IsAttackingMovePossible(positions) &&
                                (figureOnTargetPosition == null
                                || figureOnTargetPosition.Color == figureToMove.Color)
                            )
                       ))
                {
                    return NormalChessMoveValidationResult.MovePositionsAreNotValid;
                }
            }
            else
            {
                if (figureToMove.AreMovePositionsPossible(positions) == false)
                {
                    return NormalChessMoveValidationResult.MovePositionsAreNotValid;
                }
            }

            if (figureToMove is IUnableToJumpFigure)
            {
                foreach (var position in ((IUnableToJumpFigure)figureToMove).GetPositionsInTheWayOfMove(positions))
                {
                    if (this.chessBoard.GetFigureOnPosition(position) != null)
                    {
                        return NormalChessMoveValidationResult.ThereAreOtherPiecesOnTheWay;
                    }
                }
            }

            ChessBoard chessBoard = this.chessBoard.GetVirtualChessBoardAfterMove(positions);

            if (this.CheckForCheck(chessBoard, color))
            {
                return NormalChessMoveValidationResult.MovementResultsInCheckOfTheFriendlyKing;
            }

            return NormalChessMoveValidationResult.ValidMove;

        }

        private bool CheckForCheck(ChessBoard chessBoard, ChessColors defensiveColor)
        {
            for (char horizontal = 'a'; horizontal <= 'h'; horizontal++)
            {
                for (int vertical = 1; vertical <= 8; vertical++)
                {
                    var figure = chessBoard.GetFigureOnPosition(new ChessBoardPosition(horizontal, vertical));
                    if (figure == null || figure.Color == defensiveColor)
                    {
                        continue;
                    }

                    List<ChessBoardPosition> positionsAttacked =
                        this.PossiblePositionsMovement(figure.GetType(), new ChessBoardPosition(horizontal, vertical), this.GetOppositeColor(defensiveColor)
                        , chessBoard)
                        ;

                    foreach (var position in positionsAttacked)
                    {
                        IFigure figureOnPosition = chessBoard.GetFigureOnPosition(position);
                        if (figureOnPosition is King && figureOnPosition.Color == defensiveColor)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool CheckForMate(ChessColors AttackingColor)
        {
            ChessColors defensiveColor = this.GetOppositeColor(AttackingColor);
            return this.CheckIfPlayerHasNoValidMove(defensiveColor) && this.CheckForCheck(this.chessBoard, defensiveColor);
        }

        private bool CheckForDraw()
        {
            if (this.CheckIfPlayerHasNoValidMove(ChessColors.Black) && (this.CheckForCheck(this.chessBoard, ChessColors.Black) == false))
            {
                return true;
            }
            if (this.CheckIfPlayerHasNoValidMove(ChessColors.White) && (this.CheckForCheck(this.chessBoard, ChessColors.White) == false))
            {
                return true;
            }

            return false;
        }

        private bool CheckIfPlayerHasNoValidMove(ChessColors Color)
        {
            for (char i = 'a'; i <= 'h'; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    IFigure figureFromDefensiveSide = this.chessBoard.GetFigureOnPosition(new ChessBoardPosition(i, j));

                    if (figureFromDefensiveSide == null || figureFromDefensiveSide.Color == this.GetOppositeColor(Color))
                    {
                        continue;
                    }

                    for (char horizontal = 'a'; horizontal <= 'h'; horizontal++)
                    {
                        for (int vertical = 1; vertical <= 8; vertical++)
                        {
                            if (this.ValidateMove(new NormalChessMovePositions(i, j
                                , horizontal, vertical),
                                figureFromDefensiveSide.GetType()
                        , Color) == NormalChessMoveValidationResult.ValidMove)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        private List<ChessBoardPosition> PossiblePositionsMovement(Type figure, ChessBoardPosition position, ChessColors colors, ChessBoard chessBoardParam)
        {
            List<ChessBoardPosition> positions = new List<ChessBoardPosition>();
            for (char horizontal = 'a'; horizontal <= 'h'; horizontal++)
            {
                for (int vertical = 1; vertical <= 8; vertical++)
                {
                    if (this.ValidateMoveWithoutCheck(new NormalChessMovePositions(position.Horizontal, position.Vertical, horizontal, vertical)
                        , figure, colors, chessBoardParam) == NormalChessMoveValidationResult.ValidMove)
                    {
                        positions.Add(new ChessBoardPosition(horizontal, vertical));
                    }
                }
            }

            return positions;
        }

        private NormalChessMoveValidationResult ValidateMoveWithoutCheck(NormalChessMovePositions positions, Type FigureType, ChessColors color, ChessBoard chessBoardParam)
        {
            IFigure figureToMove = chessBoardParam.GetFigureOnPosition(positions.InitialPosition);
            IFigure figureOnTargetPosition = chessBoardParam.GetFigureOnPosition(positions.TargetPosition);
            if (figureToMove == null || figureToMove.GetType() != FigureType || figureToMove.Color != color)
            {
                return NormalChessMoveValidationResult.ThereIsntSuchFigureAndColorOnTheGivenPosition;
            }
            if (figureOnTargetPosition != null &&
                (figureOnTargetPosition.Color == figureToMove.Color))
            {
                return NormalChessMoveValidationResult.TheFigureOnTheTargetPositionIsFriendlyOrEnemyKing;
            }

            if (figureToMove is Pawn)
            {

                if (((Pawn)figureToMove).AreMovePositionsPossible(positions) == true
                    && figureOnTargetPosition != null)
                {
                    return NormalChessMoveValidationResult.MovePositionsAreNotValid;
                }

                if (
                        ((((Pawn)figureToMove).AreMovePositionsPossible(positions) == false &&
                    ((Pawn)figureToMove).IsAttackingMovePossible(positions) == false) ||
                            (
                                ((Pawn)figureToMove).IsAttackingMovePossible(positions) &&
                                (figureOnTargetPosition == null
                                || figureOnTargetPosition.Color == figureToMove.Color)
                            )
                       ))
                {
                    return NormalChessMoveValidationResult.MovePositionsAreNotValid;
                }
            }
            else
            {
                if (figureToMove.AreMovePositionsPossible(positions) == false)
                {
                    return NormalChessMoveValidationResult.MovePositionsAreNotValid;
                }
            }

            if (figureToMove is IUnableToJumpFigure)
            {
                foreach (var position in ((IUnableToJumpFigure)figureToMove).GetPositionsInTheWayOfMove(positions))
                {
                    if (chessBoardParam.GetFigureOnPosition(position) != null)
                    {
                        return NormalChessMoveValidationResult.ThereAreOtherPiecesOnTheWay;
                    }
                }
            }

            return NormalChessMoveValidationResult.ValidMove;
        }

        private void ProducePawn(ChessBoardPosition positionOnTheBoard, Type producedFigureType, ChessColors color)
        {
            IFigure figure = (IFigure)Activator.CreateInstance(producedFigureType, color);

            if (figure is ICastleableFigure)
            {
                ((ICastleableFigure)figure).Move();
            }

            this.chessBoard.PutFigureOnPosition(positionOnTheBoard, figure);
        }

        private Type ConvertFromFigureTypeEnumToActualType(ChessFigureType chessFigureType)
        {
            return this.GetType().Assembly.GetType($"ChessGameLogic.ChessFigures.{chessFigureType}");
        }

        private Type ConvertFromChessFigureProductionTypeEnumToActualType(ChessFigureProductionType chessFigureType)
        {
            return this.GetType().Assembly.GetType($"ChessGameLogic.ChessFigures.{chessFigureType}");
        }
    }
}
