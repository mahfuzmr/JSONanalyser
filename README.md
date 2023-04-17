# JSONanalyser
Test Project to Analyse given JSON data
## Class description:

The Article class represents a single Beer article, containing information such as its ID, 
a short description, price, unit of measurement, price per unit text, and an image.

The Beer class contains additional information about a Bear, including the Beer's ID, brand name, name, a list of articles that belong to the Beer class, 
and a description text.

The Article class can be used on its own to represent a single Beer, while the Beer class can be used to group related articles together as part of a product.

## Usage
JSONanalyser can perform the following operations on a JSON file:

* Find the maximum price per-liter from the given dataset
* Find the minimum price per-liter from the given dataset
* Find the exact product data for a specific value.
* Find the product that has the most bottle count.
* Find all the answer at atime.

## Hint: 
Given JSON data should be structured like the following

```
{
id: 1138,
brandName: "Name",
name: "Product name",
articles: [
          {
          id: xxxx,
          shortDescription: "20 x 0,5L (Glas)",
          price: 17.99,
          unit: "Liter",
          pricePerUnitText: "(1,80 €/Liter)",
          image: "https://image.urlto.xx/arti/small/xxxx.png"
          }
      ]
},
{......}
```
