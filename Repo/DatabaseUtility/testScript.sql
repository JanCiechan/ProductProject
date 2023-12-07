SELECT DISTINCT
    p.SKU
FROM
    Product p
JOIN
    Inventory i ON p.SKU = i.sku
JOIN
    Prices pr ON p.SKU = pr.SKU;
