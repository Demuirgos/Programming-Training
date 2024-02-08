open System


type Trie = {
        IsEndOfWord : bool
        Children : Map<char, Trie> 
    } 
    with static member Empty = { IsEndOfWord = false; Children = Map.empty } 
    
    
let Insert (word : string)  (trie: Trie) =
    let chars = word |> Seq.toList
    let rec Loop trie chars = 
        match chars with 
        | c::rest -> 
            let isEndOfWord = rest |> List.isEmpty
            let newChildren = 
                Map.change c  (fun value -> 
                    let newNode = 
                        match value with 
                        | Some subTrie -> Loop subTrie rest
                        | None -> Loop Trie.Empty rest
                    Some { newNode with IsEndOfWord = isEndOfWord }
                ) trie.Children
            {
                trie with Children = newChildren
            }
        | [] -> trie
    Loop trie chars


let Search (word : string)  (trie: Trie) =
    let chars = word |> Seq.toList
    let rec Loop trie chars = 
        match chars with 
        | c::rest -> 
            let searchNode = Map.tryFind c trie.Children
            match searchNode with 
            | Some node -> Loop node rest 
            | None -> false
        | [] -> trie.IsEndOfWord
    Loop trie chars

let trie = Trie.Empty |> Insert "ayman" |> Insert "amani"
printfn "%A" (trie |> Search "amani" )
printfn "%A" trie
