pipeline {
  agent any 

  environment {
    MAJOR_VERSION = 1
    MINOR_VERSIONS = 0
    PATCH_VERSION = 0
  }

  stages {
    stage('Create Zip file') {
      steps {
        sh './build.sh --build_version="${currentBuild.number}"'
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
