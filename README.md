## Environnement de développement

Il faut au préalable avoir installé Docker sur son poste, par exemple via Chocolatey : "choco install docker-desktop", pas besoin de Kubernetes, on peut laisser les configurations par défaut **il faut juste partager les X local drives de son poste de développement**.

Le plugin VS Code **Docker** est plutôt pratique si on veut avoir une vue d'ensemble de nos containers et volumes ou s'il on souhaite éviter d'avoir à taper certaines lignes de commandes.

Puis, dans Visual Studio 2019 **Tools > Options > Container Tools > Docker Compose**, il est préférable de sélectionner ces options :

![disable run containers on startup](https://affix-test-api.phoceis.com/img/vs_config.png)