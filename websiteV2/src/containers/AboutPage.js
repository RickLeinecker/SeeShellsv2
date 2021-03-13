import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { withRouter, HashRouter as Router, Route } from 'react-router-dom';
import AboutBar from '../components/AboutBar.js';
import Paper from '@material-ui/core/Paper';
import Button from '@material-ui/core/Button';
import ButtonGroup from '@material-ui/core/ButtonGroup';

const styles = {
    
};

class AboutPage extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return(
            <Router basename="/about">
                <AboutBar/>
                <Paper>
                    {this.props.subpage === "about" &&
                        <Paper>
                        </Paper>
                    }
                    {this.props.subpage === "registry" &&
                        <Paper>
                        </Paper>
                    }
                    {this.props.subpage === "shellbags" &&
                        <Paper>
                        </Paper>
                    }
                    {this.props.subpage === "parser" &&
                        <Paper>
                        </Paper>
                    }
                    {this.props.subpage === "analysis" &&
                        <Paper>
                        </Paper>
                    }
                    {this.props.subpage === "timeline" &&
                        <Paper>
                        </Paper>
                    }
                    {this.props.subpage === "filters" &&
                        <Paper>
                        </Paper>
                    }
                    {this.props.subpage === "case-studies" &&
                        <Paper>
                        </Paper>
                    }
                </Paper>
            </Router>
        );
    }
}

export default withStyles(styles)(withRouter(AboutPage));