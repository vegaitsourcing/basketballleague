# basketballleague

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Website development for a Recreational Basketball League Liga za rekreativce (https://www.facebook.com/Liga-za-rekreativce-497454943671856/) 

Basketball recreationists league is aimed at everyone interested in playing basketball within one organized competition, except for those who play basketball professionally and who are actively involved in playing basketball in one of the clubs. As the league is getting bigger, it would be great to have a website that would include both the information about the current basketball season and the information about the matches played so far. Also, it should include the data about all the players who have been playing in the League so far, as well as the statistics, that is, the players’ performance.

## Table of Contents

- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Setup](#setup)
- [Technologies](#technologies)
- [License](#license)

## Getting Started

Use these instructions to get the project up and running.

### Prerequisites

You will need the following tools:

* [Visual Studio 2017-2019](https://www.visualstudio.com/downloads/)
* [.NET Framework 4.5.2](https://dotnet.microsoft.com/download/dotnet-framework) (or higher)
* [NPM](https://nodejs.org/en/)
* [Gulp](https://gulpjs.com/)
* [node-gyp](https://github.com/nodejs/node-gyp)
* [Python](https://www.python.org/) (Required as a package dependency for `node-gyp`)

### Setup

Follow these steps to get your development environment set up:

  1. Clone the repository
  1. Go to `LZRNS.Web` directory and run: `npm install` followed by `gulp build`
  1. Build solution in Visual Studio (2017 or higher)
  1. (Optional) Configure database `connectionString` found in `LZRNS.Web/Web.config`
  1. Start Application with `LZRNS.Web` set as your StartUp Project

> Note: Visual Studio will by default try to do some of these steps when restoring package upon which you might get error related to Node version. Solution to this can be found [here](https://stackoverflow.com/questions/43849585/update-node-version-in-visual-studio-2017) (*you can always install it locally though the command line as mentioned above*)

## Technologies

* .NET Framework 4.5.2
* UmbracoCMS 7.12.2
* Entity Framework 6.2.0

## License

- **[MIT license](http://opensource.org/licenses/mit-license.php)**
- Copyright (c) 2018 [Vega IT Sourcing](https://www.vegaitsourcing.rs/)