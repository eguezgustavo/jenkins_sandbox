def versionNumber = ""
final LOCAL_FILE_NAME = "last-version.txt"
final REMOTE_FILE_NAME = "artifacts/" + BRANCH_NAME + "/" + LOCAL_FILE_NAME
final BUCKET_NAME = "jenkins-test-6756"

pipeline {
  agent any 

  parameters {
    string(description: 'Major version', name: 'MAJOR')
    string(description: 'Minor version', name: 'MINOR')
    string(description: 'Patch', name: 'PATCH')
  }

  stages {
    stage("Create Build Number") {
      steps {
        withAWS(credentials: "aws_only", region: "us-east-2") {
          script {

            if (params.MAJOR && params.MINOR && params.PATCH) {
              versionNumber = "${params.MAJOR}.${params.MINOR}.${params.PATCH}.${BUILD_NUMBER}"
              writeFile(file: LOCAL_FILE_NAME, text: "${params.MAJOR}.${params.MINOR}.${params.PATCH}")
              s3Upload(file: LOCAL_FILE_NAME, bucket: BUCKET_NAME, path: REMOTE_FILE_NAME)
              currentBuild.displayName = versionNumber
            } else {
              fileExistsOnAWS = s3DoesObjectExist(bucket: BUCKET_NAME, path: REMOTE_FILE_NAME)
              if (fileExistsOnAWS) {
                s3Download(file: LOCAL_FILE_NAME, bucket: BUCKET_NAME, path: REMOTE_FILE_NAME, force: true)
                versionNumber = readFile(LOCAL_FILE_NAME) + "." + BUILD_NUMBER
                currentBuild.displayName = versionNumber
              } else {
                error('Failing build because no version has been set')
              }
            }

          }
        }
      }
    }

    stage('Create Zip file') {
      steps {
        sh './build.sh --build-version="${versionNumber}"'
      }
    }
  }
}
