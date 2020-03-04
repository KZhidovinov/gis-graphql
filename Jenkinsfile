pipeline{
    agent { label 'master' }
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
}