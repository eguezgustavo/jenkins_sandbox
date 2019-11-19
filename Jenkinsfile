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
        withEnv(['NEW_BUILDS_ALL_TIME=${NEW_BUILDS_ALL_TIME}']) {
          version = VersionNumber(
            versionNumberString: '${MAJOR_VERSION}.${MINOR_VERSIONS}.${PATCH_VERSION}.${BUILDS_ALL_TIME}'
          );

          sh './build.sh --build_version="$version"'
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
}
