import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { withRouter, HashRouter as Router } from 'react-router-dom';
import Paper from '@material-ui/core/Paper';
import DocumentationBar from '../components/DocumentationBar.js';

const styles = {
    documentationPage: {
        display: 'flex',
        height: '100%',
        width: '100%',
    },
    sidebarContainer: {
        width: '20%',
        height: '100%',
        backgroundColor: '#424242',
        margin: '0px',
        display: 'flex',
        flexDirection: 'column',
    },
    primaryButtons: {
        display: 'flex',
        justifyContent: 'left',
        color: '#F2F2F2',
        '&:hover': {
            color: '#33A1FD',
        },
        fontSize: '30px',
        fontFamily: 'Georgia',
    },
    buttons: {
        display: 'flex',
        justifyContent: 'center',
        color: '#F2F2F2',
        '&:hover': {
            color: '#33A1FD',
        },
        fontSize: '20px',
        fontFamily: 'Georgia',
    },
};

class DocumentationPage extends React.Component {
    render() {
        return(
            <Router basename="/documentation">
                <DocumentationBar/>
                <Paper>
                    {this.props.subpage === "documentation" &&
                        <Paper>
                        </Paper>
                    }
                    {this.props.subpage === "online" &&
                        <Paper>
                        </Paper>
                    }
                    {this.props.subpage === "offline" &&
                        <Paper>
                        </Paper>
                    }
                    {this.props.subpage === "advanced" &&
                        <Paper>
                        </Paper>
                    }
                    {this.props.subpage === "toolbar" &&
                        <Paper>
                        </Paper>
                    }
                    {this.props.subpage === "data" &&
                        <Paper>
                        </Paper>
                    }
                    {this.props.subpage === "timeline" &&
                        <Paper>
                        </Paper>
                    }
                    {this.props.subpage === "events" &&
                        <Paper>
                        </Paper>
                    }
                    {this.props.subpage === "filters" &&
                        <Paper>
                        </Paper>
                    }
                    {this.props.subpage === "export" &&
                        <Paper>
                        </Paper>
                    }
                    {this.props.subpage === "import" &&
                        <Paper>
                        </Paper>
                    }
                    {this.props.subpage === "licensing" &&
                        <Paper>
                        </Paper>
                    }
                </Paper>
            </Router>
        );
    }
}

export default withStyles(styles)(withRouter(DocumentationPage));