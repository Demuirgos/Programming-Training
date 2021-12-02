open System.IO
type Instruction = Up of int | Down of int | Forward of int | NoMove
type Part = Part1 | Part2
type State = {
    Horizontal : int
    Aim : int
    Depth : int
}
let init = {Horizontal = 0; Depth = 0; Aim = 0}
let instructions =  File.ReadLines("Day2.txt")
                    |> Seq.map (fun e -> e.Trim().Split ' ')
                    |> Seq.map (fun e -> match e.[0] with 
                                    | "up" -> Up(int e.[1])
                                    | "forward" -> Forward(int e.[1])
                                    | "down" -> Down(int e.[1])
                                    | _ -> NoMove)
                    |> Seq.toList
let rec apply state mode =
    function 
    | [] -> state
    | h::t -> 
    match mode with 
        | Part1 -> 
            match h with 
            | Up(v) -> apply { state with Depth = state.Depth - v } mode t 
            | Down(v) -> apply { state with Depth = state.Depth + v } mode t 
            | Forward(v) -> apply { state with  Horizontal = state.Horizontal + v } mode t
            | NoMove -> state
        | Part2 -> 
            match h with
            | Up(v) -> apply { state with Aim = state.Aim - v } mode t
            | Down(v) -> apply { state with Aim = state.Aim + v } mode t
            | Forward(v) -> apply { state with  Horizontal = state.Horizontal + v 
                                                Depth = state.Depth + state.Aim * v} mode t
            | NoMove -> state

let results = 
    let result1 = apply init Part1 instructions
    let result2 = apply init Part2 instructions
    {|Part1 = result1.Depth * result1.Horizontal ; 
      Part2 = result2.Depth * result2.Horizontal|}
