pipeline {
  agent any 
  stages {
    stage('Stage 1') {
      steps {
        sh 'echo "Stage 1 Step 1"'
        sh 'echo "Stage 1 Step 2"'
      }
    }
    stage('Upload to S3') {
      steps {
        withAWS(credentials: 'aws_only', region: 'us-east-2') {
          s3Upload(file: 'README.md', bucket: 'jenkins-test-6756', path: 'artifacts/')
        }
      }
    }
  }
}
