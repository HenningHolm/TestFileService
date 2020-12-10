https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/

#### Lag ny migration

```ps
Add-Migration -name {pending number}_{name} -verbose
```

#### Rull tilbake til spesifikk migration

```ps
Update-Database InitialEmpty
```

#### Slett siste migration

```ps
Remove-Migration
```

#### Rull ut siste migration

```ps
Update-Database
```