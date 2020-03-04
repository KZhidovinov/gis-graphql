pipeline {
  agent {
    docker {
      image 'mcr.microsoft.com/dotnet/core/sdk:3.1-alpine'
      args '-v $WORKSPACE:/src'
    }

  }
  stages {
    stage('Restore') {
      steps {
        sh 'printenv'
        sh 'cd /src && dotnet restore --configfile nuget.config --verbosity normal'
      }
    }

    stage('Build') {
      steps {
        sh 'printenv'
        sh 'cd /src && dotnet build --no-restore --configuration $CONFIGURATION --verbosity normal'
      }
    }

    stage('Test') {
      steps {
        sh 'printenv'
        sh 'cd /src && dotnet test --no-build --configuration $CONFIGURATION --verbosity normal'
      }
    }

  }
  environment {
    HOME = '/tmp'
    DOTNET_CLI_TELEMETRY_OPTOUT = 'true'
    DOTNET_SKIP_FIRST_TIME_EXPERIENCE = 'true'
    CONFIGURATION = 'Release'
  }
}