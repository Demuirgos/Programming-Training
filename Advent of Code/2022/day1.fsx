open System.IO
open System

let isDigit (str : string) = (not (Seq.isEmpty str)) && (Seq.forall Char.IsDigit str)
isDigit ""

let path = @".\day1.txt"
let lines = File.ReadAllLines(path) |> Array.toList
let getElfRations inputs = 
    let rec loop acc subacc input= 
        match input with 
        | x :: xs when isDigit x-> 
            if x = "" then printf "Empty string"
            loop acc (int x::subacc) xs
        | x :: xs -> loop (subacc::acc) [] xs
        | _ -> acc
    loop [] [] inputs

let elfRations = getElfRations lines
let fatElfs = elfRations |> List.map (fun x -> x |> List.sum) |> List.sortDescending
let fattestElfRation = fatElfs.Head
let topThreeFatElfs = fatElfs |> List.take 3 |> List.sum 