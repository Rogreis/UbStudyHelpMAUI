## Amadon Search Engine - Help Guide

### Introduction
Amadon search engine is based on [Apache Lucene](https://lucene.apache.org/core/2_9_4/queryparsersyntax.html), a powerful search library, which can provide advanced search functionality to any application. This help guide will walk you through the basics of using the Lucene.net based search engine in our application.

![The search engine](images/Search.png)

### Getting Started

1.  **Input Your Search Query**: Type your search term in the search box and press enter. You can search for a single word, a phrase, or use advanced queries.

### Search Operators

Our search engine supports several search operators:

1.  **AND**: This operator matches documents where both terms exist anywhere in the text. To search for documents that contain "term1" and "term2", use: `term1 AND term2`
    
2.  **OR**: This operator matches documents where either term exists anywhere in the text. To search for documents that contain "term1" or "term2", use: `term1 OR term2`
    
3.  **NOT**: This operator excludes documents that contain the term after NOT. To search for documents that contain "term1" but not "term2", use: `term1 NOT term2`
    
4.  **"+": (Must)**: This operator makes a term mandatory, or "must appear" in the result. To search for documents that must contain "term1", use: `+term1`
    
5.  **"-": (Must Not)**: This operator requires that the term must not appear in the document. To search for documents that must not contain "term1", use: `-term1`
    
6.  **"*": (Wildcard)**: The wildcard operator can be used to search for partial words. For instance, to find documents containing words that start with "term", use: `term*`
    
7.  **"~": (Fuzzy Search)**: This operator can be used when you are not sure of the correct spelling. To perform a fuzzy search, use: `term~`
    
8.  **"": (Phrase)**: This operator is used to search for exact phrases. For instance, to find documents containing the exact phrase "my phrase", use: `"my phrase"`
    

### Search Tips

1.  Lucene.net search is case insensitive. For example, searching for "term" will also return "Term" or "TERM".
    
2.  Special characters need to be escaped with "". For example, to search for "(1+1)=2", use: `\(1\+1\)=2`
    
3.  Avoid using a single wildcard (*) as a search term, as it may return too many results and slow down the search.
    

### Troubleshooting

If you're having issues with the search functionality, try these steps:

1.  Check your search term for typos or spelling mistakes.
    
2.  If you're using search operators, ensure that they're formatted correctly.
    
3.  If the search is returning too many irrelevant results, try adding more specific terms or using the AND or "+" operator to narrow down the results.
    
4.  If the search is returning too few or no results, try using broader terms or the OR operator to widen the search.
    

Remember, the aim of the search is to help you find exactly what you're looking for in the most efficient way. Happy searching!

### Basic search examples:

- Jesus
- "Christ Michael"
- Jesus and God
- "My master delays his coming" AND God
- Adora???
- Adora*

### Wildcards

The single character wildcard search looks for terms that match that with the single character replaced. For example, to search for "absoluta" or "absolute" you can use the search:

- absolut?

Multiple character wildcard searches looks for 0 or more characters. For example, to search for absoluta, absolute or absoluteness, you can use the search:

- absolut*

You can also use the wildcard searches in the middle of a term.

- abso*e will find everything starting with abso and finishing with e

### Similar (Fuzzy) Searches

To do a similar search use the tilde, "~", symbol at the end of a Single word Term. For example to search for a term similar in spelling to "abso" use the fuzzy search:

- roam~ will find terms like foam and roams.
 

An additional (optional) parameter can specify the required similarity. The value is between 0 and 1, with a value closer to 1 only terms with a higher similarity will be matched. For example:

- roam~0.8

*The default that is used if the parameter is not given is 0.5.*

### Proximity Searches

You can try finding words are a within a specific distance away. To do a proximity search use the tilde, "~", symbol at the end of a Phrase. For example to search for a "Jesus" and "God" within 10 or 2 words of each other in a document use the search:

- "Jesus God"~10 finds 117 paragraphs

- "Jesus God"~2 finds 18 paragraphs

### Boosting a Term

To increase some term's relevance use the caret, "^", symbol with a boost factor (a number) at the end of the term you are searching. The higher the boost factor, the more relevant the term will be.

Boosting allows you to control the relevance of a document by boosting its term. For example, if you are searching for **Jesus God** and you want the term "**Jesus**" to be more relevant boost it using the ^ symbol along with the boost factor next to the term. You would type:

- Jesus^4 God This will make documents with the term Jesus appear more relevant.

- "Jesus Christ"^4 "apostle Peter" Here the phrase has a bigger boost

By default, the boost factor is 1. Although the boost factor must be positive, it can be less than 1 (e.g. 0.2)
 

### Boolean Operators

Boolean operators allow terms to be combined through logic operators. This search supports AND, "+", OR, NOT and "-" as Boolean operators(Note: Boolean operators must be ALL CAPS).

  

The OR operator is the default conjunction operator. This means that if there is no Boolean operator between two terms, the OR operator is used. The OR operator links two terms and finds a matching document if either of the terms exist in a document. This is equivalent to a union using sets. The symbol || can be used in place of the word OR.

  

#### OR

To search for documents that contain either "Christ Michael" or just "Jesus" use the query:

- "Christ Michael" OR Jesus

#### AND

The AND operator matches documents where both terms exist anywhere in the text of a single document. This is equivalent to an intersection using sets. The symbol && can be used in place of the word AND.

To search for documents that contain **"Christ Michael"** and **"Jesus of Nazareth"** use the query:

- "Christ Michael" AND "Jesus of Nazareth"


#### +

The "+" or required operator requires that the term after the "+" symbol exist somewhere in a the field of a single document.

To search for documents that must contain **"Christ "** and may contain **"Nebadon"** use the query:

- +Christ Nebadon

#### NOT

The NOT operator excludes documents that contain the term after NOT. This is equivalent to a difference using sets. The symbol ! can be used in place of the word NOT.

To search for documents that contain **"Christ Michael"** but not **"Jesus of Nazareth"** use the query:

- "Christ Michael" NOT "Jesus of Nazareth"

Note: The NOT operator cannot be used with just one term. For example, the following search will return no results: **NOT "Christ Michael"**

#### -

  

The "-" or prohibit operator excludes documents that contain the term after the "-" symbol.

  

To search for documents that contain **"Christ Michael"** but not **"Jesus of Nazareth"** use the query:

  

- "Christ Michael" - "Jesus of Nazareth"

  
  

### Grouping

  

You can use parentheses to group clauses to form sub queries. This can be very useful if you want to control the boolean logic for a query.

  

To search for either "Christ " or "Nebadon" and "Gabriel" use the query:

  

- (Christ OR Nebadon) AND Gabriel

  
  
  

*This eliminates any confusion and makes sure you that website must exist and either term Christ or Nebadon may exist.*

  

### Field Grouping

  

Parentheses also can be used to group multiple clauses to a single field.

  

To search for a title that contains both the word "Buddha" and the phrase "Genghis Khan" use the query:

  

- (+Buddha+"Genghis Khan")

  
  

### Escaping Special Characters

  

Lucene supports escaping special characters that are part of the query syntax. The current list special characters are

  

+  - && || ! ( ) { } [ ] ^ " ~ * ? : \

  

To escape these character use the \ before the character. For example to search for **[Revealed** use the query:

  

-  \\[Revealed




> Written with [StackEdit](https://stackedit.io/).
<!--stackedit_data:
eyJoaXN0b3J5IjpbNjg5MDcyMTM1LC0xNDA0Mzc5OTQ2LDEzNz
AxMDI4NzMsMTU1NTQ1NDY1MF19
-->