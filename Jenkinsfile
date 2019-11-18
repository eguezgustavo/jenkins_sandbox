pipeline {
  agent any 
  stages {
    stage('Stage 1') {
      steps {
        sh 'echo "Stage 1 Step 1"'
        sh 'echo "Stage 1 Step 2"'
      }
    }
    stage('Stage 2') {
      steps {
        withAWS(credentials: 'aws_only', region: 'us-east-1') {
          s3Upload(file: 'README.md', bucket: 'some-bucket', path: 'artifacts/')
        }
      }
    }
  }
}
