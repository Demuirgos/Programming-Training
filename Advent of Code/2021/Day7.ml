let sum l = List.fold_right (+) l 0 
let median l = 
  let lenght = List.length l in 
  lenght / 2
let abs i = if i < 0 then -i else i
let solve data = 
  let input = List.sort (fun i j -> i - j) data in
  let medin = List.nth input (median input) in 
  sum (List.map (fun i -> abs (i - medin)) input)
