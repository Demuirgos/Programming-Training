open System.IO
type Instruction = Up of int | Down of int | Forward of int | NoMove
type State = {
    Horizontal : int
    Depth : int
}
let init = {Horizontal = 0; Depth = 0}
let instructions =  File.ReadLines(@"./Input/Day2.txt")
                    |> Seq.map (fun e -> e.Trim().Split ' ')
                    |> Seq.map (fun e -> match e.[0] with 
                                    | "up" -> Up(int e.[1])
                                    | "forward" -> Forward(int e.[1])
                                    | "down" -> Down(int e.[1])
                                    | _ -> NoMove)
                    |> Seq.toList
let rec apply state = 
    function 
    | [] -> state
    | h::t -> 
        match h with 
        | Up(v) -> apply { state with Depth = state.Depth - v } t
        | Down(v) -> apply { state with Depth = state.Depth + v } t
        | Forward(v) -> apply { state with Horizontal = state.Horizontal + v } t
        | NoMove -> state

let part1 = 
    let result = apply init instructions
    result.Depth * result.Horizontal 
