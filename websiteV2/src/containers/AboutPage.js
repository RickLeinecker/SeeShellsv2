import React from 'react';
import { withStyles, withTheme } from '@material-ui/core/styles';
import { withRouter, HashRouter as Router, Route } from 'react-router-dom';
import AboutBar from '../components/AboutBar.js';
import Paper from '@material-ui/core/Paper';
import Typography from '@material-ui/core/Typography';
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
    video: {
        maxHeight: '360px',
        maxWidth: '640px',
        minHeight: '150px',
        minWidth: '300px',
        height: '50%',
        width: '50%',
        margin: '10px',
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        backgroundColor: '#424242',
    },
    caseStudies: {
        display: 'flex',
        justifyContent: 'space-evenly',
        flexWrap: 'wrap',
        height: '100%',
        width: '100%',
        overflow: 'auto',
    },
    caseStudy: {
        textAlign: 'center',
        fontSize: '30px',
        fontWeight: 'bold',
        color: 'white',
    },
};

class AboutPage extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return(
            <Router basename="/about">
                <AboutBar/>
                <Paper className={this.props.classes.content}>
                    {this.props.subpage === "about" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>About</Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                SeeShells is an information extraction software. SeeShells is a standalone, 
                                open source executable that can run both online and offline registry hives. 
                                It extracts and parses through Windows Registry information. This data is then 
                                converted into two forms. The first is a csv file that contains all the raw data 
                                we obtain. The second is a human readable timeline. The timeline provides an 
                                interactive, easier to read visualization of the data extracted from the windows registries. 
                                This information is otherwise difficult and time consuming to comb through and understand. 
                                The application is a great way to gain insight into what someone has done on their computer over time. 
                                The program can be particularly useful for digital forensics investigators as the information 
                                can be downloaded and used as evidence in a court of law. 
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                Windows uses the Shellbag keys to store user preferences for GUI folder display within Windows Explorer. 
                                Everything from visible columns to display mode (icons, details, list, etc.) to sort order are tracked. 
                                If you have ever made changes to a folder and returned to that folder to find your new preferences intact, 
                                then you have seen Shellbags in action. 
                            </Typography>
                            <Paper className={this.props.classes.video}>
                                <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=IZrd86723Hc"/>
                            </Paper>
                        </Paper>
                    }
                    {this.props.subpage === "registry" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>The Windows Registry</Typography>
                        </Paper>
                    }
                    {this.props.subpage === "shellbags" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>Shellbags</Typography>
                        </Paper>
                    }
                    {this.props.subpage === "seeshells" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>SeeShells</Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                SeeShells is an information extraction software. The objective is to create a standalone open 
                                source executable that can run both online and offline. It extracts and parses through Windows 
                                Registry information. This data is then converted into two forms. The first is a csv file that 
                                contains all the raw data we obtain from the registry. The second is a human readable timeline 
                                that can be downloaded and used as evidence in a courtroom. The timeline provides an interactive 
                                easier to read visualization of the data extracted from the windows registries. This information 
                                is otherwise difficult and time consuming to comb through and understand. The timeline can be 
                                filtered by date, event name, the contents of the event (e.g. accessed, modified, created), user, 
                                and the event type. These filters can be applied to all events and cleared out individually as the 
                                users see fit. The application also contains an about page as well as a help page so that users who 
                                are not able to connect to the internet are still able to use the program and obtain guidance if the need it.
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                The parsing and extraction of information has a slightly different process for each of the windows 
                                versions including Windows XP, Windows Vista Windows 7,8,8.1 and 10. In order to create a robust 
                                application we have set up a server to store database information on parsing different registry 
                                versions. We have implemented the use of embedded scripting in order to keep the application 
                                up-to-date without requiring the users to update the program or redownload it. Currently, we do not 
                                know all there is to know about shellbags. Currently unidentifiable shellbag items check if a script exists to parse it.
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                The software expediates the process of extracting, parsing, and presenting the registry information 
                                in a way that is condensed and easily understandable. We hope others will benefit from our interactive 
                                timeline generated from the ShellBag information and we hope to make a great impact on the digital forensics community.
                            </Typography>
                        </Paper>
                    }
                    {this.props.subpage === "parser" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>The SeeShells Parser</Typography>
                        </Paper>
                    }
                    {this.props.subpage === "analysis" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>SeeShells Shellbag Analysis</Typography>
                        </Paper>
                    }
                    {this.props.subpage === "timeline" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>SeeShells Timeline View</Typography>
                        </Paper>
                    }
                    {this.props.subpage === "filters" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>SeeShells Shellbag Filtering</Typography>
                        </Paper>
                    }
                    {this.props.subpage === "case-studies" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>Case Studies</Typography>
                            <Paper className={this.props.classes.caseStudies}>
                                <Paper className={this.props.classes.video}>
                                    <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=IZrd86723Hc"/>
                                    <Typography variant="title" className={this.props.classes.caseStudy}>Case Study: title</Typography>
                                </Paper>
                                <Paper className={this.props.classes.video}>
                                    <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=IZrd86723Hc"/>
                                    <Typography variant="title" className={this.props.classes.caseStudy}>Case Study: title</Typography>
                                </Paper>
                                <Paper className={this.props.classes.video}>
                                    <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=IZrd86723Hc"/>
                                    <Typography variant="title" className={this.props.classes.caseStudy}>Case Study: title</Typography>
                                </Paper>
                                <Paper className={this.props.classes.video}>
                                    <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=IZrd86723Hc"/>
                                    <Typography variant="title" className={this.props.classes.caseStudy}>Case Study: title</Typography>
                                </Paper>
                                <Paper className={this.props.classes.video}>
                                    <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=IZrd86723Hc"/>
                                    <Typography variant="title" className={this.props.classes.caseStudy}>Case Study: title</Typography>
                                </Paper>
                                <Paper className={this.props.classes.video}>
                                    <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=IZrd86723Hc"/>
                                    <Typography variant="title" className={this.props.classes.caseStudy}>Case Study: title</Typography>
                                </Paper>
                            </Paper>
                        </Paper>
                    }
                </Paper>
            </Router>
        );
    }
}

export default withStyles(styles)(withRouter(AboutPage));