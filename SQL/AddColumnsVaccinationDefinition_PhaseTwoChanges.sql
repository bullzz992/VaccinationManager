alter table VaccinationDefinitions
add ICDCode varchar(100) null
go

alter table VaccinationDefinitions
add Price decimal(18,2) null default(0.00)
go