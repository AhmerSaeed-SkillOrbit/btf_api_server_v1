pipeline  {
    agent { label 'ubuntu' }
    stages {
        stage('Build & UnitTest') {
            steps {
                sh "docker build -t burmataskforce ."
            }
        }
        stage('Integration Test') {
            steps {
                sh "docker run -t burmataskforce"
            }
        }
    }  
}
