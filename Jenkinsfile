pipeline {
  agent none
  stages {
    stage('Build') {
      agent {
        docker {
          image 'mcr.microsoft.com/dotnet/core/sdk:3.1-alpine'
          args '-v $WORKSPACE:/src'
        }

      }
      environment {
        HOME = '/tmp'
        DOTNET_CLI_TELEMETRY_OPTOUT = 'true'
        DOTNET_SKIP_FIRST_TIME_EXPERIENCE = 'true'
      }
      steps {
        sh '''cd /src

printenv

dotnet restore --configfile nuget.config \\
  --verbosity normal 
'''
      }
    }

  }
}