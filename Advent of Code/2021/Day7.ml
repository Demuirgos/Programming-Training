open Core.Std
open Prevasives
type mode = 
    Part1 
    | Part2
let sum l = List.fold_right (+) l 0 
let median l = 
  let lenght = List.length l in 
  lenght / 2
let mean l = 
  let add, len = sum l, List.length l in
  add / len 
let abs i = if i < 0 then -i else i
let consumn n = n * (n + 1) / 2
let solve mode =
  let data = List.map int_of_string ( List.flatten (In_channel.read_lines ".\Input\Day7.txt")) in
  let input = List.sort (fun i j -> i - j) data in
  match mode with
      Part1 -> 
    	let medin = List.nth sorted (median input) in 
    	let result = sum (List.map (fun i -> abs (i - medin)) input) in
    	result
      | Part2 ->
    	let mean = mean input in 
    	let getFuel v = sum (List.map (fun i -> consumn (abs (i - v))) input) in
    	let pot1, pot2 = getFuel mean, getFuel (mean + 1) in
    	let result = if pot1 > pot2 then pot2 else pot1 in
    	result
