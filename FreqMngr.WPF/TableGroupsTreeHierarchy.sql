UPDATE TableGroups SET depth = 0 
WHERE parentId IS NULL;

WHILE EXISTS (SELECT * FROM TableGroups WHERE Depth IS NULL) 
	UPDATE T SET T.Depth = P.Depth + 1
		FROM TableGroups AS T INNER JOIN TableGroups AS P 
        ON (T.ParentId = P.Id) 
    WHERE P.Depth >= 0 
    AND T.Depth IS NULL;

UPDATE TableGroups SET PathIndex = 0, NumericalMapping = '0.0'
WHERE ParentId IS NULL;

;WITH x AS 
(
    SELECT Id, rank() over (partition by ParentId order by Id) as PathIndex 
    FROM TableGroups 
    WHERE ParentId IS NOT NULL  
)
UPDATE TableGroups 
SET PathIndex = x.PathIndex
FROM x 
WHERE TableGroups.Id = x.Id;

UPDATE TableGroups
SET Numericalmapping = PathIndex 
WHERE depth = 1;

WHILE EXISTS (SELECT * FROM TableGroups WHERE NumericalMapping Is Null) 
    UPDATE T SET T.NumericalMapping =   cast(P.Numericalmapping as 
                                            varchar(256)) + '.' + 
                                        cast(T.PathIndex as varchar(256))  
		FROM TableGroups AS T INNER JOIN TableGroups AS P 
		ON (T.ParentId = P.Id) 
    WHERE P.PathIndex >= 0 
    AND T.NumericalMapping IS NULL;  


WITH Items(Id, Name, ParentId, Depth, PathIndex, ItemNumber) 
AS 
(
	SELECT Id, Name, ParentId, depth, PathIndex, NumericalMapping
	FROM TableGroups
) 
SELECT * 
FROM Items 
ORDER BY CONVERT(hierarchyid, '/' + ItemNumber + '/')