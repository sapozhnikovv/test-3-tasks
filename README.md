# test-3-tasks
[Задания для собеседования 6-5-25](https://disk.yandex.ru/i/FEdTwIqvUgOHWA)

# CV
[Резюме визитка краткое](https://github.com/sapozhnikovv/test-3-tasks/blob/main/%D0%A1%D0%B0%D0%BF%D0%BE%D0%B6%D0%BD%D0%B8%D0%BA%D0%BE%D0%B2%D0%92%D1%8F%D1%87%D0%B5%D1%81%D0%BB%D0%B0%D0%B2_short_cv.pdf)
[Резюме с hh](https://github.com/sapozhnikovv/test-3-tasks/blob/main/hh_cv_%D0%A0%D0%B5%D0%B7%D1%8E%D0%BC%D0%B5_Senior_Software_Developer_NET_C_Tech_Lead_%D0%92%D1%8F%D1%87%D0%B5%D1%81%D0%BB%D0%B0%D0%B2_%D0%A1%D1%82%D0%B0%D0%BD%D0%B8%D1%81%D0%BB%D0%B0%D0%B2%D0%BE%D0%B2%D0%B8%D1%87.pdf)

## Задание 1
Компрессия и декомпрессия с предвычислением будущего размера
[StringCompression.cs](https://github.com/sapozhnikovv/test-3-tasks/blob/main/SimpleStringCommpression/Algo/StringCompression.cs) 
тесты - [UnitTest.cs](https://github.com/sapozhnikovv/test-3-tasks/blob/main/SimpleStringCommpression/AlgoTest/UnitTest1.cs)

## Задание 2
Реализовано 2 решения.

Первое решение - то что ожидается по заданию [NormalServer.cs](https://github.com/sapozhnikovv/test-3-tasks/blob/main/FastAndSafeCounter/NormalServer/NormalServer.cs)

Тесты - [UnitTest.cs](https://github.com/sapozhnikovv/test-3-tasks/blob/main/FastAndSafeCounter/NormalServerTest/UnitTest1.cs)


Второе решение - быстрее в  5-30 раз **за счёт использования Lock-free подхода и кольцевого буфера, где каждый элемент буфера расположен на отдельной кеш линии процессора, так получается что к каждой кеш-линии имеет доступ меньшее кол-во потоков и нет трат времени**.

Решение - [HighLoadServer.cs](https://github.com/sapozhnikovv/test-3-tasks/blob/main/FastAndSafeCounter/HighLoadServer/HighLoadServer.cs)

Тесты - [UnitTest.cs](https://github.com/sapozhnikovv/test-3-tasks/blob/main/FastAndSafeCounter/HighLoadServerTest/UnitTest1.cs)
![tests](https://github.com/sapozhnikovv/test-3-tasks/blob/main/FastAndSafeCounter/test.jpg)
### Общий бенчмарк

**Environment:**  
- BenchmarkDotNet v0.14.0  
- Windows 10 (10.0.19042.1586/20H2/October2020Update)  
- **CPU:** 11th Gen Intel Core i5-11320H 3.20GHz (1 CPU, 8 logical, 4 physical cores)  
- **.NET SDK:** 8.0.400  
- **Host:** .NET 8.0.14 (X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI)  

| Method                                   | Mean (μs) | Error (μs) | StdDev (μs) | Gen0   | Allocated |
|------------------------------------------|----------:|-----------:|------------:|-------:|----------:|
| **NormalServer_**                        |          |            |             |        |           |
| `ConcurrentReadsNoWrites`                | 2,250.6  | 5.07       | 4.75        | -      | 4.25 KB   |
| `ConcurrentWritesNoReads`                | 3,507.9  | 36.12      | 33.78       | -      | 4.18 KB   |
| `ReadAndWrite`                           | 6,141.5  | 121.32     | 285.96      | -      | 7.59 KB   |
| **HighLoadServer_**                      |          |            |             |        |           |
| `ConcurrentReadsNoWrites`                | 186.9    | 3.69       | 3.62        | 0.7324 | 2.94 KB   |
| `ConcurrentWritesNoReads`                | 110.9    | 1.20       | 1.43        | 0.7324 | 2.94 KB   |
| `ReadAndWrite`                           | 973.8    | 18.39      | 19.68       | -      | 5.85 KB   |

### Key Observations
- **HighLoadServer** значительно быстрее **NormalServer** (в ~10–30 раз).  
- Наибольшая разница в `ConcurrentWritesNoReads`: **HighLoadServer** быстрее в ~31.6 раз (`3507.9 μs` vs `110.9 μs`).  
- При смешанной нагрузке (`ReadAndWrite`) разница ~6.3 раз (`6141.5 μs` vs `973.8 μs`).  
- **HighLoadServer** использует немного больше памяти (Gen0 коллекции), но общий объём аллокаций ниже.  


## Задание 3
Обработчик - [Processor.cs](https://github.com/sapozhnikovv/test-3-tasks/blob/main/LogStandardizer/LogStandardizer/Processor.cs)
Файлы - [LogStandardizer/files](https://github.com/sapozhnikovv/test-3-tasks/tree/main/LogStandardizer/LogStandardizer/files)
