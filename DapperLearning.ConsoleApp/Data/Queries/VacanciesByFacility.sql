SELECT DISTINCT
    v.*
    ,q.* 
    ,f.* 
    ,fa.*
FROM dbo.Vacancy                           as v 
INNER JOIN dbo.VacancyQualificationMapping as vq ON v.Id = vq.VacancyId
INNER JOIN dbo.Qualification               as q  ON q.Id = vq.QualificationId
INNER JOIN dbo.Facility                    as f  ON f.Id = v.FacilityId
LEFT JOIN  dbo.FacilityArea                as fa ON fa.Id = v.FacilityAreaId
