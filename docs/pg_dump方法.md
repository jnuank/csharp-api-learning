
```sh
pg_dump -h <host名> -U <user> -d <databaes名> --data-only --no-comments --no-privileges --inserts --schema=todo 
```


少し要らないもの削る

```sh
pg_dump -h <host名> -U <user> -d <databaes名> --data-only --no-comments --no-privileges --inserts --schema=todo | rg -v "SET" | rg -v "^--"  | rg -v "^$" | rg -v SELECT 
```