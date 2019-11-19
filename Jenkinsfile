pipeline {
  agent any 
  stages {
    stage('Create Zip file') {
      steps {
        sh './build.sh --build_version="11.0.1.2"'
      }
    }
    // stage('Upload to S3') {
    //   steps {
    //     withAWS(credentials: 'aws_only', region: 'us-east-2') {
    //       s3Upload(workingDir: 'something', bucket: 'jenkins-test-6756', path: 'artifacts/', includePathPattern:'**/*.zip')
    //     }
    //   }
    // }
  }
}
