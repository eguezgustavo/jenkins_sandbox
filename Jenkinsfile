def versionNumber = ""

pipeline {
  agent any 

  parameters {
    string(defaultValue: "1", description: 'Major version', name: 'MAJOR')
    string(defaultValue: "0", description: 'Minor version', name: 'MINOR')
    string(defaultValue: "0", description: 'Patch', name: 'PATCH')
  }

  stages {
    stage('Create Build Number') {
      steps {
        withAWS(credentials: 'aws_only', region: 'us-east-2') {
          s3Download(file: 'versions.json', bucket: 'jenkins-test-6756', path: 'artifacts/')
        }
      }
    }

    stage('Create Zip file') {
      steps {
        // sh './build.sh --build_version="${versionNumber}"'
        echo "hola mundo ${params.MAJOR}.${params.MINOR}.${params.PATCH}"
      }
    }
  }
}
