pipeline {
  agent any 

  parameters {
    string(description: 'Major version', name: 'MAJOR')
    string(description: 'Minor version', name: 'MINOR')
    string(description: 'Patch', name: 'PATCH')
  }

  stages {
    stage("Win") {
      steps {
        dir('win') {
          sh 'ls -al'
        }
      }
    }

    stage("Net") {
      steps {
        dir('net') {
          sh 'ls -al'
        }
      }
    }
  }
}
