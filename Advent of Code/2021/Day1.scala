import scala.io.Source
val InputPath = "./Input/Day1.txt"
val Reports   = Source.fromFile(InputPath).getLines().map(int).toList
def part1(r: List[Int]) = r.tail.zip(r).filter((v) => v._1 > v._2).length
def part2(r: List[Int]) = r.tail.tail.zip(r.tail).zip(r).map((v) => List(v._1._1, v._1._2, v._2).sum)
val result = println(part1(part2(Reports)))