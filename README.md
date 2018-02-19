# GeUtilities

Genome Utilities (GeUtilities) provides open-source building-blocks for genomic data analysis tools. The following components are currently implemented: 
-	**IGenomics**: interfaces to build portable objects. For instance, ChIP-seq peaks, variations, or general features. 
-	**Parsers**; highly customizable parsers reading source files into in-memory objects. The following parsers are currently implemented:
    - Interval-based data formats
        - Browser Extensible Data (BED)
        - Gene transfer format (GTF)
        - Variant Call Format (VCF)
        - Reference Sequence (RefSeq)

These components are highly customizable making them suitable for variety of application scenarios and variations of input data. For instance, you can use a parser as simple as passing the path to the file to be parsed and call `Parse()` function. However, if you have a tool that implements a class `Foo` for a ChIP-seq peak, you can set the parser to read BED files and produce peaks in the `Foo` type. Therefore, no need to case or convert from the parsed data type to your application’s implemented types. Additionally, if the file to be parsed has different column orders than what the format specification says (e.g., _p-value_ is given on the second column of a “BED” file), then you can update the parser’s column indexes to match your data. Moreover, you may want to only **sniff** your data (i.e., read only the first 10 lines of the input), then you can specify the number of lines you want the parser to read. Accordingly, our design decisions enable delivering components that can be used out-of-box with minimal configurations while still highly customizable. 

## Build and Test Status of the Repository

|                    | x64 Release |
| :----------------- | :---------- |
| Windows            | [![Build status](https://ci.appveyor.com/api/projects/status/4pyyaxw3bx87yyd9?svg=true)](https://ci.appveyor.com/project/VJalili/geutilities) |
| Linux Ubuntu 14.04 | [![Build Status](https://travis-ci.org/Genometric/GeUtilities.svg?branch=travis.yml)](https://travis-ci.org/Genometric/GeUtilities) |


## Code Quality Metrics

[![measure](https://sonarcloud.io/api/project_badges/measure?project=geutilities&metric=alert_status)](https://sonarcloud.io/dashboard/index/geutilities)     [![codecov](https://codecov.io/gh/Genometric/GeUtilities/branch/master/graph/badge.svg)](https://codecov.io/gh/Genometric/GeUtilities)

[![measure](https://sonarcloud.io/api/project_badges/measure?project=geutilities&metric=sqale_rating)](https://sonarcloud.io/dashboard/index/geutilities)     [![measure](https://sonarcloud.io/api/project_badges/measure?project=geutilities&metric=reliability_rating)](https://sonarcloud.io/dashboard/index/geutilities)     [![measure](https://sonarcloud.io/api/project_badges/measure?project=geutilities&metric=security_rating)](https://sonarcloud.io/dashboard/index/geutilities)     [![measure](https://sonarcloud.io/api/project_badges/measure?project=geutilities&metric=ncloc)](https://sonarcloud.io/dashboard/index/geutilities)     [![measure](https://sonarcloud.io/api/project_badges/measure?project=geutilities&metric=bugs)](https://sonarcloud.io/dashboard/index/geutilities)     [![measure](https://sonarcloud.io/api/project_badges/measure?project=geutilities&metric=code_smells)](https://sonarcloud.io/dashboard/index/geutilities)     [![measure](https://sonarcloud.io/api/project_badges/measure?project=geutilities&metric=vulnerabilities)](https://sonarcloud.io/dashboard/index/geutilities)
