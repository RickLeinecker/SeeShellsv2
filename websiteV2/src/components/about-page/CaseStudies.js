import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { Typography, Paper, IconButton, Tooltip } from '@material-ui/core';
import PictureAsPdfIcon from '@material-ui/icons/PictureAsPdf';
import Widgets from '@material-ui/icons/Widgets';
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
        textAlign: 'center',
    },
    text: {
        textAlign: 'center',
        padding: '1%',
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
    button: {
        color: '#33A1FD',
    }
}

class CaseStudies extends React.Component {

    render() {
        return (
            <div className={this.props.classes.content}>
                <Typography variant="title" className={this.props.classes.title}>Case Studies</Typography>
                <Paper className={this.props.classes.caseStudies}>
                    <Typography variant="subtitle1" className={this.props.classes.text}>
                        The following videos are a series of case studies. Provided below are PDF walkthroughs that supplement the videos, and UsrClass.dat 
                        files used for each case study, that can be downloaded and imported into SeeShells.
                    </Typography>
                    <Paper className={this.props.classes.video}>
                        <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=fzK-bUQrIxg"/>
                        <Typography variant="title" className={this.props.classes.caseStudy}>Case Study: Tehsla IP Theft</Typography>
                        <div className={this.props.classes.buttonContainer}>
                            <Tooltip title="Case Study PDF">
                                <a href="https://drive.google.com/file/d/12I4JLMh5s0AxGLfoolCt6zaRASO-14pe/view?usp=sharing">
                                    <IconButton className={this.props.classes.button}>
                                        <PictureAsPdfIcon/>
                                    </IconButton>
                                </a>
                            </Tooltip>
                            <Tooltip title="Case Study UsrClass.dat">
                                <a href="https://drive.google.com/file/d/1PlNHog2XiECRZw8zZMNkPPHuIlB9i4HF/view?usp=sharing">
                                    <IconButton className={this.props.classes.button}>
                                        <Widgets/>
                                    </IconButton>
                                </a>
                            </Tooltip>
                        </div>
                    </Paper>
                    <Paper className={this.props.classes.video}>
                        <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=n38WWu_T49Q"/>
                        <Typography variant="title" className={this.props.classes.caseStudy}>Case Study: Arasaka Phishing Incident</Typography>
                        <div className={this.props.classes.buttonContainer}>
                            <Tooltip title="Case Study PDF">
                                <a href="https://drive.google.com/file/d/1RF2tL91YBeg78UbKvUdFgb2-eJHZY3g3/view?usp=sharing">
                                    <IconButton className={this.props.classes.button}>
                                        <PictureAsPdfIcon/>
                                    </IconButton>
                                </a>
                            </Tooltip>
                            <Tooltip title="Case Study UsrClass.dat">
                                <a href="https://drive.google.com/file/d/1OZHhfAvACc93mWxJEcjyI7jwkAFo4Jv9/view?usp=sharing">
                                    <IconButton className={this.props.classes.button}>
                                        <Widgets/>
                                    </IconButton>
                                </a>
                            </Tooltip>
                        </div>
                    </Paper>
                    <Paper className={this.props.classes.video}>
                        <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=dp1xxl1BGcE"/>
                        <Typography variant="title" className={this.props.classes.caseStudy}>Case Study: Hooli Workstation Analysis</Typography>
                        <div className={this.props.classes.buttonContainer}>
                            <Tooltip title="Case Study PDF">
                                <a href="https://drive.google.com/file/d/1mm3CeIlwpl4YVg7YiJMfB-KpE0XtlJ9C/view?usp=sharing">
                                    <IconButton className={this.props.classes.button}>
                                        <PictureAsPdfIcon/>
                                    </IconButton>
                                </a>
                            </Tooltip>
                            <Tooltip title="Case Study UsrClass.dat">
                                <a href="https://drive.google.com/file/d/1XuevQkHZa86btb3ZJ63Sh7mdyGL8xq98/view?usp=sharing">
                                    <IconButton className={this.props.classes.button}>
                                        <Widgets/>
                                    </IconButton>
                                </a>
                            </Tooltip>
                        </div>
                    </Paper>
                </Paper>
            </div>
        )
    }
}

export default withStyles(styles)(CaseStudies);