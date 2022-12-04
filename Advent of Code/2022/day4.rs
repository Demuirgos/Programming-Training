use std::fs::File;
use std::io::{self, BufRead, Error};
struct Range(i32, i32);

fn read_file(path: &str) -> io::Result<Vec<String>> {
    let mut file = File::open(path).expect("Unable to open file");
    let test = Ok(io::BufReader::new(file).lines().filter_map(|line| 
        match line {
            Ok(line) => Some(line),
            _ => None
        }
    ).collect());
    test
}

fn extract_range(line : &str) -> Vec<Range> {
    let parts = line.split(',');
    parts.into_iter()
        .map(|part| part.split('-').collect())
        .map(|split : Vec<_>| Range(
            split[0].parse::<i32>().unwrap(), 
            split[1].parse::<i32>().unwrap())).collect::<Vec<_>>()
}

fn check_overlap(ranges: &[Range]) -> (bool, bool) {
    match ranges {
        [left, right] =>
            (
                (left.0 <= right.1) && (left.1 >= right.0),
                (left.0 <= right.0 && left.1 >= right.1) ||  (left.0 >= right.0 && left.1 <= right.1)
            ),
        _ => panic!("unexpected number of args")
    }
}

fn main() {
    if let Ok(lines) = read_file("./day3.txt") {
        let partSelection = 2;
        let result : usize = lines.iter()
            .map(|line| 
                extract_range(line)
                    .chunks(2)
                    .map(check_overlap)
                    .filter_map(|result| match (partSelection, result) { 
                        (1, (_, true)) => Some(1), 
                        (2, (true, _)) => Some(1), 
                        _ => None
                    })
                    .count())
            .sum::<usize>();
        println!("{}", result);    
    }
} 