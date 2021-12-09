Imports System
Imports System.IO
Imports System.Collections.Generic
Imports System.Linq
Module Program

    Structure Cell
        public Sub New(V As Integer, M As Boolean)
            Value = V
            Mark  = M
        End Sub
        Public Value As Integer
        Public Mark As Boolean
    End Structure
    Class Game
        Public Boards As List(Of Board) 
        Public Tape   As List(Of Integer)
        public Sub New(B As List(Of Board), T As List(Of Integer))
            Boards = B
            Tape   = T
        End Sub
        Public Function Process(Part As Integer)
            Dim Winners As List(Of (Integer, Integer)) = new List(Of (Integer, Integer))
            Dim Card As Integer
            For Index = 0 To Tape.Count - 1
                Card = Tape(Index)
                For Each board As Board In Boards
                    board.Mark(Card)
                Next
                For i As Integer = 0 To Boards.Count - 1
                    Dim board  = Boards(i)
                    If Not Winners.Exists(Function(e) e.Item1 = i) Then
                        If board.Check() Then
                            If Part = 1 Then
                                Return PostProcess(board, Card)
                            Else 
                                Winners.Add((i, Card))
                            End If 
                        End If
                    End If
                Next i
                If Winners.Count = Boards.Count Then
                    Exit For 
                End If
            Next
            Dim Result = Winners(Winners.Count - 1)
            Return PostProcess(Boards(Result.Item1), Result.Item2)
        End Function
        Private Function PostProcess(Winner As Board, Last As Integer)
            Dim Result As Integer = 0
            For Each Cell in Winner.Matrix
                If Cell.Mark = False Then
                    Result = Result + Cell.Value
                End If
            Next
            Return Result * Last
        End Function 
    End Class
    Class Board
        Public Sub New(M(,) As Cell)
            Matrix = M
        End Sub
        Public Matrix(4, 4) As Cell
        Public Function Mark(Number as Integer)
            For Y_Index = 0 To Matrix.GetUpperBound(0)
                For X_Index = 0 To Matrix.GetUpperBound(1)
                    If Matrix(Y_Index, X_Index).Value = Number Then
                        Me.MarkCell(Y_Index, X_Index)
                    End If
                Next
            Next
            Return Me
        End Function
        Public Sub MarkCell(Y_Index as Integer, X_Index as Integer) 
            Matrix(Y_Index, X_Index).Mark = True
        End Sub 
        Public Function Check()
            For Y_Index = 0 To Matrix.GetUpperBound(0)
                Dim HResult As Boolean = True
                Dim VResult As Boolean = True
                For X_Index = 0 To Matrix.GetUpperBound(1)
                    HResult = HResult And Matrix(Y_Index, X_Index).Mark
                    VResult = VResult And Matrix(X_Index, Y_Index).Mark
                Next
                If VResult Or HResult Then
                    Return True
                End If
            Next
            Return False
        End Function
    End Class
    Class Parser
        Private Reader As StreamReader  
        Public Function ParseLine()
            Dim Line As String = Reader.ReadLine()
            Return (From token In Split(Line, ",")
                    Select Convert.ToInt32(token)).ToList()
        End Function
        Public Function ParseMatrix()
            Dim Matrix(4, 4) As Cell
            Reader.ReadLine()
            For i As Integer = 0 To Matrix.GetUpperBound(0) 
                Dim Line = Reader.ReadLine() 
                Dim Tokens = (From token in Split(Line) Where token <> "").ToList()
                For j As Integer = 0 To Matrix.GetUpperBound(1)
                    Matrix(i ,j) = New Cell(Convert.ToInt32(Tokens(j)), False)
                Next
            Next
            Return New Board(Matrix)
        End Function
        Public Function ParseInput(Path As String)
            Reader = New StreamReader(Path)
            Dim Cards = ParseLine()
            Dim Boards As List(of Board) = New List(of Board)
            While reader.Peek() <> -1 
                Boards.Add(ParseMatrix())
            End While
            Return New Game(Boards, Cards)
        End Function
    End Class
    Sub Main(args As String())
        Dim Parser = New Parser()
        Dim Game   = Parser.ParseInput("Input\\Day4.txt") 
        Dim Result = Game.Process(2)
        Console.WriteLine(Result)
    End Sub
End Module
