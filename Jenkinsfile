pipeline{
    agent { 
        docker {
            image 'mcr.microsoft.com/dotnet/core/sdk:3.1-alpine'
            label 'sdk'
        }
    }
    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }
        stage('Tests') {
            steps {
                sh 'dotnet test -c Release'
            }
        }
    }

    stage('Tests') {
      steps {
        sh 'dotnet test -c Release'
      }
    }
}