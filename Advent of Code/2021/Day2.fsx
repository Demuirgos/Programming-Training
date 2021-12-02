type Instruction = Up of int | Down of int | Forward of int | NoMove
type State = {
    Horizontal : int
    Depth : int
}
let init = {Horizontal = 0; Depth = 0}
let instructions =   """forward 5
                        down 5
                        forward 8
                        up 3
                        down 8
                        forward 2""".Split '\n'
                    |> Array.map (fun e -> e.Trim().Split ' ')
                    |> Array.map (fun e -> match e.[0] with 
                                    | "up" -> Up(int e.[1])
                                    | "forward" -> Forward(int e.[1])
                                    | "down" -> Down(int e.[1])
                                    | _ -> NoMove)
                    |> Array.toList
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
