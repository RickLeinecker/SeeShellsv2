import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { withRouter, HashRouter as Router } from 'react-router-dom';
import Paper from '@material-ui/core/Paper';
import DocumentationBar from '../components/DocumentationBar.js';
import Typography from '@material-ui/core/Typography';
import Slide from '@material-ui/core/Slide';
import Collapse from '@material-ui/core/Collapse';
import Button from '@material-ui/core/Button';
import ArrowDropDownIcon from '@material-ui/icons/ArrowDropDown';
import ReactPlayer from "react-player";

const styles = {
    content: {
        height: '100%',
        width: '100%',
        display: 'flex',
        justifyContent: 'flex-start',
        flexDirection: 'column',
        alignItems: 'center',
        overflow: 'auto',
        borderRadius: '0',
    },
    title: {
        fontSize: '50px',
        fontWeight: 'bold',
        marginTop: '1%',
        alignSelf: 'center',
        color: '#33A1FD',
    },
    text: {
        textAlign: 'center',
        padding: '1%',
    },
    sidebarContainer: {
        height: '100%',
        display: 'flex',
        alignContent: 'center',
        flexDirection: 'column',
        borderRadius: '0',
    },
    toggle: {
        backgroundColor: '#424242',
        borderRadius: '0',
    },
    topBar: {
        width: '100%',
        display: 'flex',
        justifyContent: 'center',
        backgroundColor: '#424242',
        alignContent: 'center',
        flexDirection: 'column',
        borderRadius: '0',
    },
    buttons: {
        color: 'white',
    }
};

class DocumentationPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            showBar: true,
            dropdown: false,
        };

        this.toggleDropdown = this.toggleDropdown.bind(this);
    }

    toggleDropdown() {
        this.setState({ dropdown: !this.state.dropdown });
    }

    componentDidMount() {
        window.addEventListener("resize", this.resize.bind(this));
        this.resize();
    }
    
    resize() {
        this.setState({hideNav: window.innerWidth <= 760});
    }
    
    componentWillUnmount() {
        window.removeEventListener("resize", this.resize.bind(this));
    }

    render() {
        return(
            <Router basename="/documentation">
                {!this.state.hideNav &&
                    <Slide in={this.state.showBar} direction="right" mountOnEnter unmountOnExit>
                        <Paper className={this.props.classes.sidebarContainer}>
                            <DocumentationBar/>
                        </Paper>
                    </Slide>
                }
                <Paper elevation={0} className={this.props.classes.content}>
                    {this.state.hideNav &&
                        <Paper className={this.props.classes.topBar}>
                            <Button className={this.props.classes.buttons} onClick={this.toggleDropdown}><ArrowDropDownIcon/></Button>
                            <Collapse in={this.state.dropdown}>
                                <Paper>
                                    <DocumentationBar/>
                                </Paper>
                            </Collapse>
                        </Paper>
                    }
                    {this.props.subpage === "documentation" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>How To Use</Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                SeeShells collects ShellBags specific Windows Registry keys and parses through them, 
                                and organizes the data found in them to display them on a graphical timeline. The 
                                graphical timeline is the unique feature that SeeShells offers over other existing parsers: 
                                this timeline makes ShellBag data easier to understand and facilitates the process of
                                finding a significant pattern or piece of evidence.
                            </Typography>
                        </Paper>
                    }
                    {this.props.subpage === "online" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>Online Parsing</Typography>
                        </Paper>
                    }
                    {this.props.subpage === "offline" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>Offline Parsing</Typography>
                        </Paper>
                    }
                    {this.props.subpage === "advanced" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>Advanced Configuration</Typography>
                        </Paper>
                    }
                    {this.props.subpage === "toolbar" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>Toolbar</Typography>
                        </Paper>
                    }
                    {this.props.subpage === "data" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>Viewing The Data</Typography>
                        </Paper>
                    }
                    {this.props.subpage === "timeline" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>Timeline View</Typography>
                        </Paper>
                    }
                    {this.props.subpage === "events" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>Shellbag Events</Typography>
                        </Paper>
                    }
                    {this.props.subpage === "filters" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>Shellbag Filtering</Typography>
                        </Paper>
                    }
                    {this.props.subpage === "export" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>Exporting</Typography>
                        </Paper>
                    }
                    {this.props.subpage === "import" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>Importing</Typography>
                        </Paper>
                    }
                    {this.props.subpage === "licensing" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>Licensing</Typography>
                        </Paper>
                    }
                </Paper>
            </Router>
        );
    }
}

export default withStyles(styles)(withRouter(DocumentationPage));