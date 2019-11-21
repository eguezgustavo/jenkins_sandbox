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
            print("**************************")
            print(params.MAJOR)
            print("**************************")
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
