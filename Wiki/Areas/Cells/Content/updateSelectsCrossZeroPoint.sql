--А-Д
update [SCells].[dbo].[SectionMap] set [SCells].[dbo].[SectionMap].distance = [SCells].[dbo].[SelectionAtoZeroPoint].distance + [SCells].[dbo].[SelectionDtoZeroPoint].distance
FROM [SCells].[dbo].[SectionMap] 
left join [SCells].[dbo].[SelectionAtoZeroPoint] on [SCells].[dbo].[SelectionAtoZeroPoint].sectionIdStart = [SCells].[dbo].[SectionMap].[sectionIdStart]
left join [SCells].[dbo].[SelectionDtoZeroPoint] on [SCells].[dbo].[SelectionDtoZeroPoint].sectionIdStart = [SCells].[dbo].[SectionMap].[sectionIdFinish]
where
[SCells].[dbo].[SelectionAtoZeroPoint].idS is not null
and
[SCells].[dbo].[SelectionDtoZeroPoint].idS is not null
--Д-А
update [SCells].[dbo].[SectionMap] set [SCells].[dbo].[SectionMap].distance = [SCells].[dbo].[SelectionAtoZeroPoint].distance + [SCells].[dbo].[SelectionDtoZeroPoint].distance
FROM [SCells].[dbo].[SectionMap] 
left join [SCells].[dbo].[SelectionAtoZeroPoint] on [SCells].[dbo].[SelectionAtoZeroPoint].sectionIdStart = [SCells].[dbo].[SectionMap].[sectionIdFinish]
left join [SCells].[dbo].[SelectionDtoZeroPoint] on [SCells].[dbo].[SelectionDtoZeroPoint].sectionIdStart = [SCells].[dbo].[SectionMap].[sectionIdStart]
where
[SCells].[dbo].[SelectionAtoZeroPoint].idS is not null
and
[SCells].[dbo].[SelectionDtoZeroPoint].idS is not null

--А-Г
update [SCells].[dbo].[SectionMap] set [SCells].[dbo].[SectionMap].distance = [SCells].[dbo].[SelectionAtoZeroPoint].distance + [SCells].[dbo].[SelectionGtoZeroPoint].distance
FROM [SCells].[dbo].[SectionMap] 
left join [SCells].[dbo].[SelectionAtoZeroPoint] on [SCells].[dbo].[SelectionAtoZeroPoint].sectionIdStart = [SCells].[dbo].[SectionMap].[sectionIdStart]
left join [SCells].[dbo].[SelectionGtoZeroPoint] on [SCells].[dbo].[SelectionGtoZeroPoint].sectionIdStart = [SCells].[dbo].[SectionMap].[sectionIdFinish]
where
[SCells].[dbo].[SelectionAtoZeroPoint].idS is not null
and
[SCells].[dbo].[SelectionGtoZeroPoint].idS is not null
--Г-А
update [SCells].[dbo].[SectionMap] set [SCells].[dbo].[SectionMap].distance = [SCells].[dbo].[SelectionAtoZeroPoint].distance + [SCells].[dbo].[SelectionGtoZeroPoint].distance
FROM [SCells].[dbo].[SectionMap] 
left join [SCells].[dbo].[SelectionAtoZeroPoint] on [SCells].[dbo].[SelectionAtoZeroPoint].sectionIdStart = [SCells].[dbo].[SectionMap].[sectionIdFinish]
left join [SCells].[dbo].[SelectionGtoZeroPoint] on [SCells].[dbo].[SelectionGtoZeroPoint].sectionIdStart = [SCells].[dbo].[SectionMap].[sectionIdStart]
where
[SCells].[dbo].[SelectionAtoZeroPoint].idS is not null
and
[SCells].[dbo].[SelectionGtoZeroPoint].idS is not null

--Б-Д
update [SCells].[dbo].[SectionMap] set [SCells].[dbo].[SectionMap].distance = [SCells].[dbo].[SelectionDtoZeroPoint].distance + [SCells].[dbo].[SelectionBtoZeroPoint].distance
FROM [SCells].[dbo].[SectionMap] 
left join [SCells].[dbo].[SelectionBtoZeroPoint] on [SCells].[dbo].[SelectionBtoZeroPoint].sectionIdStart = [SCells].[dbo].[SectionMap].[sectionIdStart]
left join [SCells].[dbo].[SelectionDtoZeroPoint] on [SCells].[dbo].[SelectionDtoZeroPoint].sectionIdStart = [SCells].[dbo].[SectionMap].[sectionIdFinish]
where
[SCells].[dbo].[SelectionBtoZeroPoint].idS is not null
and
[SCells].[dbo].[SelectionDtoZeroPoint].idS is not null
--Д-Б
update [SCells].[dbo].[SectionMap] set [SCells].[dbo].[SectionMap].distance = [SCells].[dbo].[SelectionDtoZeroPoint].distance + [SCells].[dbo].[SelectionBtoZeroPoint].distance
FROM [SCells].[dbo].[SectionMap] 
left join [SCells].[dbo].[SelectionBtoZeroPoint] on [SCells].[dbo].[SelectionBtoZeroPoint].sectionIdStart = [SCells].[dbo].[SectionMap].[sectionIdFinish]
left join [SCells].[dbo].[SelectionDtoZeroPoint] on [SCells].[dbo].[SelectionDtoZeroPoint].sectionIdStart = [SCells].[dbo].[SectionMap].[sectionIdStart]
where
[SCells].[dbo].[SelectionBtoZeroPoint].idS is not null
and
[SCells].[dbo].[SelectionDtoZeroPoint].idS is not null

--Б-Г
update [SCells].[dbo].[SectionMap] set [SCells].[dbo].[SectionMap].distance = [SCells].[dbo].[SelectionGtoZeroPoint].distance + [SCells].[dbo].[SelectionBtoZeroPoint].distance
FROM [SCells].[dbo].[SectionMap] 
left join [SCells].[dbo].[SelectionBtoZeroPoint] on [SCells].[dbo].[SelectionBtoZeroPoint].sectionIdStart = [SCells].[dbo].[SectionMap].[sectionIdStart]
left join [SCells].[dbo].[SelectionGtoZeroPoint] on [SCells].[dbo].[SelectionGtoZeroPoint].sectionIdStart = [SCells].[dbo].[SectionMap].[sectionIdFinish]
where
[SCells].[dbo].[SelectionBtoZeroPoint].idS is not null
and
[SCells].[dbo].[SelectionGtoZeroPoint].idS is not null
--Г-Б
update [SCells].[dbo].[SectionMap] set [SCells].[dbo].[SectionMap].distance = [SCells].[dbo].[SelectionGtoZeroPoint].distance + [SCells].[dbo].[SelectionBtoZeroPoint].distance
FROM [SCells].[dbo].[SectionMap] 
left join [SCells].[dbo].[SelectionBtoZeroPoint] on [SCells].[dbo].[SelectionBtoZeroPoint].sectionIdStart = [SCells].[dbo].[SectionMap].[sectionIdFinish]
left join [SCells].[dbo].[SelectionGtoZeroPoint] on [SCells].[dbo].[SelectionGtoZeroPoint].sectionIdStart = [SCells].[dbo].[SectionMap].[sectionIdStart]
where
[SCells].[dbo].[SelectionBtoZeroPoint].idS is not null
and
[SCells].[dbo].[SelectionGtoZeroPoint].idS is not null

--Д-И
update [SCells].[dbo].[SectionMap] set [SCells].[dbo].[SectionMap].distance = [SCells].[dbo].[SelectionItoZeroPoint].distance + [SCells].[dbo].[SelectionDtoZeroPoint].distance
FROM [SCells].[dbo].[SectionMap] 
left join [SCells].[dbo].[SelectionDtoZeroPoint] on [SCells].[dbo].[SelectionDtoZeroPoint].sectionIdStart = [SCells].[dbo].[SectionMap].[sectionIdStart]
left join [SCells].[dbo].[SelectionItoZeroPoint] on [SCells].[dbo].[SelectionItoZeroPoint].sectionIdStart = [SCells].[dbo].[SectionMap].[sectionIdFinish]
where
[SCells].[dbo].[SelectionDtoZeroPoint].idS is not null
and
[SCells].[dbo].[SelectionItoZeroPoint].idS is not null
--И-Д
update [SCells].[dbo].[SectionMap] set [SCells].[dbo].[SectionMap].distance = [SCells].[dbo].[SelectionItoZeroPoint].distance + [SCells].[dbo].[SelectionDtoZeroPoint].distance
FROM [SCells].[dbo].[SectionMap] 
left join [SCells].[dbo].[SelectionDtoZeroPoint] on [SCells].[dbo].[SelectionDtoZeroPoint].sectionIdStart = [SCells].[dbo].[SectionMap].[sectionIdFinish]
left join [SCells].[dbo].[SelectionItoZeroPoint] on [SCells].[dbo].[SelectionItoZeroPoint].sectionIdStart = [SCells].[dbo].[SectionMap].[sectionIdStart]
where
[SCells].[dbo].[SelectionDtoZeroPoint].idS is not null
and
[SCells].[dbo].[SelectionItoZeroPoint].idS is not null

--Г-И
update [SCells].[dbo].[SectionMap] set [SCells].[dbo].[SectionMap].distance = [SCells].[dbo].[SelectionItoZeroPoint].distance + [SCells].[dbo].[SelectionGtoZeroPoint].distance
FROM [SCells].[dbo].[SectionMap] 
left join [SCells].[dbo].[SelectionGtoZeroPoint] on [SCells].[dbo].[SelectionGtoZeroPoint].sectionIdStart = [SCells].[dbo].[SectionMap].[sectionIdStart]
left join [SCells].[dbo].[SelectionItoZeroPoint] on [SCells].[dbo].[SelectionItoZeroPoint].sectionIdStart = [SCells].[dbo].[SectionMap].[sectionIdFinish]
where
[SCells].[dbo].[SelectionGtoZeroPoint].idS is not null
and
[SCells].[dbo].[SelectionItoZeroPoint].idS is not null
--И-Г
update [SCells].[dbo].[SectionMap] set [SCells].[dbo].[SectionMap].distance = [SCells].[dbo].[SelectionItoZeroPoint].distance + [SCells].[dbo].[SelectionGtoZeroPoint].distance
FROM [SCells].[dbo].[SectionMap] 
left join [SCells].[dbo].[SelectionGtoZeroPoint] on [SCells].[dbo].[SelectionGtoZeroPoint].sectionIdStart = [SCells].[dbo].[SectionMap].[sectionIdFinish]
left join [SCells].[dbo].[SelectionItoZeroPoint] on [SCells].[dbo].[SelectionItoZeroPoint].sectionIdStart = [SCells].[dbo].[SectionMap].[sectionIdStart]
where
[SCells].[dbo].[SelectionGtoZeroPoint].idS is not null
and
[SCells].[dbo].[SelectionItoZeroPoint].idS is not null