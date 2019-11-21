def versionNumber = ""

pipeline {
  agent any 

  parameters {
    string(defaultValue: "1", description: 'Major version', name: 'MAJOR')
    string(defaultValue: "0", description: 'Minor version', name: 'MINOR')
    string(defaultValue: "0", description: 'Patch', name: 'PATCH')
  }

  stages {
    stage("Create Build Number") {
      steps {
        withAWS(credentials: "aws_only", region: "us-east-2") {
          script {

            LOCAL_FILE_NAME = "last-version"
            REMOTE_FILE_NAME = "artifacts/" + "${BRANCH_NAME}/" + LOCAL_FILE_NAME
            BUCKET_NAME = "jenkins-test-6756"

            fileExistsOnAWS = s3FindFiles(bucket: "${BUCKET_NAME}", onlyFiles: true, path: "${REMOTE_FILE_NAME}")
            if (fileExistsOnAWS) {
              echo "******** FOUND"
              s3Download(file: "${LOCAL_FILE_NAME}", bucket: "${BUCKET_NAME}", path: "${REMOTE_FILE_NAME}", force: true)
            } else {
              echo "******** NOT FOUND"
            }

            versionFileExistsOnJenkins = fileExists("${LOCAL_FILE_NAME}")
            if (versionFileExistsOnJenkins) {
              versionNumber = readFile("${LOCAL_FILE_NAME}") + "." + "${BUILD_NUMBER}"
            } else {
              versionNumber = "${params.MAJOR}.${params.MINOR}.${params.PATCH}.${BUILD_NUMBER}"
              writeFile(file: "${LOCAL_FILE_NAME}", text: "${params.MAJOR}.${params.MINOR}.${params.PATCH}")                  
              s3Upload(file: "${LOCAL_FILE_NAME}", bucket: "${BUCKET_NAME}", path: "${REMOTE_FILE_NAME}")
            }

            currentBuild.displayName = versionNumber
          } 
        }
      }
    }

    stage('Create Zip file') {
      steps {
        // sh './build.sh --build_version="${versionNumber}"'
        echo "hola mundo ${versionNumber}"
      }
    }
  }
}
