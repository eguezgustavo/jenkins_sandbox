def versionNumber = ""

pipeline {
  agent any 

  environment {
    MAJOR_VERSION = "1"
    MINOR_VERSION= "0"
    PATCH_VERSION = "0"
  }

  stages {
    stage('Create Zip file') {
      steps {
        script {
            versionNumber = VersionNumber(versionNumberString: '${MAJOR_VERSION}.${MINOR_VERSION}.${PATCH_VERSION}.${BUILDS_ALL_TIME}')
            currentBuild.displayName = versionNumber
        }
        sh './build.sh --build_version="${versionNumber}"'
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
}
