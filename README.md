# Science Journal Catalogue - Project .NET Framework

* Naam: Akshat Verma
* Studentennummer: 0171120-12
* Academiejaar: 24-25
* Klasgroep: ISB204B
* Onderwerp: Scientist * - * Scientific article * - 1 Science Journal


## Sprint 1

```mermaid
classDiagram
  class Scientist
  class ScientificArticle
  class ScienceJournal

  Scientist "*" -- "*" ScientificArticle
  ScientificArticle "*" -- "1" ScienceJournal
```
