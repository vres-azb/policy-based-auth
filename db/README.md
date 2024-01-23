# Data

## Resources

- Docker:
```docker
$ docker commit <container_name> <newimagename>
$ docker run -ti -v "$PWD/somedir":/somedir <newimagename> /bin/bash

#
docker container run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=P@ssw0rd" \
--name abes-sql --hostname abes-sql \
-p 1433:1433 \
-v ./db/bak/:/var/opt/mssql/backup/ \
-d mcr.microsoft.com/mssql/server:2022-latest
#-d abes-sql
```
- [Backup and Restore](https://learn.microsoft.com/en-us/sql/linux/tutorial-restore-backup-in-sql-server-container?view=sql-server-ver16&tabs=prod)◊