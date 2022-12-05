package main

import (
	"os"
	"strconv"
)

type Item struct {
	Index int
	Value byte
}

type Pile struct {
	Items []Item
}

type State struct {
	Piles   []Pile
	isSetup bool
}

type Action struct {
	From  int
	To    int
	Count int
}

func reverse[T any](container []T) []T {
	for i := 0; i < len(container)/2; i++ {
		j := len(container) - i - 1
		container[i], container[j] = container[j], container[i]
	}
	return container
}

func chunk[T any](container []T, size int) (chunks [][]T) {
	for i := 0; i < len(container); i += size {
		chunks = append(chunks, container[i:i+size])
	}
	return
}

func chunkBy[T comparable](container []T, check func(T) bool) (chunks [][]T) {
	var j = 0
	for i := 0; i < len(container); i += 1 {
		if check(container[i]) {
			chunks = append(chunks, container[j:i])
			j = i + 1
		}
	}
	chunks = append(chunks, container[j:])
	return
}

func takeUntil[T any](container []T, check func(T) bool) (chunks []T) {
	for _, item := range container {
		if !check(item) {
			chunks = append(chunks, item)
		} else {
			break
		}
	}
	return
}

func skipWhile[T any](container []T, check func(T) bool) (chunks []T) {
	var i = 0
	for _, item := range container {
		if check(item) {
			i += 1
		} else {
			break
		}
	}
	chunks = container[i+1:]

	return
}

func mapBy[T, TResult any](container []T, mapper func(int, T) TResult) (chunks []TResult) {
	var i = 0
	for _, item := range container {
		var mapped = mapper(i, item)
		chunks = append(chunks, mapped)
		i += 1
	}
	return
}

func flatten[T any](container [][]T) (chunks []T) {
	for _, item := range container {
		chunks = append(chunks, item...)
	}
	return
}

func filterBy[T any](container []T, check func(T) bool) (chunks []T) {
	for _, item := range container {
		if check(item) {
			chunks = append(chunks, item)
		}
	}
	return
}

func (state *State) apply(setup []Item, actions []Action, part int) *State {
	if !state.isSetup {
		var max = 0
		for _, item := range setup {
			if item.Index > max {
				max = item.Index
			}
		}
		var piles = make([]Pile, max+1)
		for _, item := range setup {
			if item.Value != ' ' && item.Value != '\n' {
				piles[item.Index].Items = append([]Item{item}, piles[item.Index].Items...)
			}
		}
		state.Piles = piles
		state.isSetup = true
	}

	if len(actions) == 0 {
		return state
	}

	var action = actions[0]
	var from = action.From
	var to = action.To
	var count = action.Count
	var length = len(state.Piles[from].Items)
	if part == 1 {
		state.Piles[to].Items = append(state.Piles[to].Items, reverse(state.Piles[from].Items[length-count:])...)
	} else {
		state.Piles[to].Items = append(state.Piles[to].Items, state.Piles[from].Items[length-count:]...)
	}
	state.Piles[from].Items = state.Piles[from].Items[:length-count]
	return state.apply(setup, actions[1:], part)
}

func (state *State) print() {
	for _, pile := range state.Piles {
		print(string(pile.Items[len(pile.Items)-1].Value))
	}
}

func main() {
	data, err := os.ReadFile("day5.txt")
	if err == nil {
		var lines = chunkBy(data, func(char byte) bool {
			return char == '\n'
		})
		var setupLines = takeUntil(lines,
			func(line []byte) bool {
				return len(line) == 1
			})
		var setup = flatten(mapBy(setupLines[:len(setupLines)-1],
			func(idx int, line []byte) []Item {
				return mapBy(chunk(line, 4), func(idx int, chunk []byte) Item {
					var trimmed = chunk[:len(chunk)-1]
					return Item{
						Index: idx,
						Value: trimmed[1],
					}
				})
			}))

		var actionLines = mapBy(skipWhile(lines,
			func(line []byte) bool {
				return len(line) > 1
			}),
			func(idx int, line []byte) (result []byte) {
				if line[len(line)-1] == 13 {
					result = line[:len(line)-1]
				} else {
					result = line
				}
				return
			})
		var moves = mapBy(actionLines,
			func(idx int, line []byte) Action {
				var words = chunkBy(line, func(char byte) bool {
					return char == ' '
				})
				var Count, _ = strconv.Atoi(string(words[1]))
				var From, _ = strconv.Atoi(string(words[3]))
				var To, _ = strconv.Atoi(string(words[5]))
				return Action{
					Count: Count,
					From:  From - 1,
					To:    To - 1,
				}
			})

		var initialState = State{
			isSetup: false,
			Piles:   nil,
		}

		initialState.apply(setup, moves, 2).print()
	}
}
