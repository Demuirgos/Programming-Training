open System.IO
open System

let isDigit (str : string) = (not (Seq.isEmpty str)) && (Seq.forall Char.IsDigit str)

@".\2022\day1.txt"
|> File.ReadAllLines
|> Array.toList
|> fun inputs -> 
    let rec loop acc subacc input= 
        match input with 
        | x :: xs when isDigit x-> 
            if x = "" then printf "Empty string"
            loop acc (int x::subacc) xs
        | x :: xs -> loop (subacc::acc) [] xs
        | _ -> acc
    loop [] [] inputs
|> List.map (fun x -> x |> List.sum)
|> List.sortDescending
|> function
    | first :: second::third :: _  -> 
        let fattestElfRations = first
        let topThreeFatElfsRations = first + second + third
        printfn "Fattest elf rations: %d" fattestElfRations
    | _ -> ()
