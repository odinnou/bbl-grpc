# bbl-grpc
## Environnement de développement

Il faut au préalable installer Docker for Windows "choco install docker-desktop --version 2.1.0.5" (la version est importante, la 2.2.0.0 pose des soucis), pas besoin de Kubernetes, on peut laisser les configurations par défaut **il faut juste partager les X local drives de son poste de développement**.

Il faut ensuite créer les volumes **lsaf-sqldata** et **lsaf-elasticsearch** qui seront utilisés par les containers postgres & elastic search (pour ne pas perdre de données à chaque clean)

    docker volume create --name=grpc-sqldata