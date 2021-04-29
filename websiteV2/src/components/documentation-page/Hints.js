import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { Typography, Paper } from '@material-ui/core';
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
                <Typography variant="title" className={this.props.classes.title}>Helpful Hint Videos</Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    Select a video below for a visual overview of the docs.
                </Typography>
                <Paper className={this.props.classes.caseStudies}>
                    <Paper className={this.props.classes.video}>
                        <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=O6HnfsjFQAg"/>
                        <Typography variant="title" className={this.props.classes.caseStudy}>How To: Parse A Live Registry</Typography>
                    </Paper>
                    <Paper className={this.props.classes.video}>
                        <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=O6HnfsjFQAg"/>
                        <Typography variant="title" className={this.props.classes.caseStudy}>How To: Parse An Offline Hive</Typography>
                    </Paper>
                    <Paper className={this.props.classes.video}>
                        <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=O6HnfsjFQAg"/>
                        <Typography variant="title" className={this.props.classes.caseStudy}>How To: Use The Shell Inspector</Typography>
                    </Paper>
                    <Paper className={this.props.classes.video}>
                        <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=O6HnfsjFQAg"/>
                        <Typography variant="title" className={this.props.classes.caseStudy}>How To: Use The Event Timeline</Typography>
                    </Paper>
                    <Paper className={this.props.classes.video}>
                        <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=O6HnfsjFQAg"/>
                        <Typography variant="title" className={this.props.classes.caseStudy}>How To: Use The Shellbag Table</Typography>
                    </Paper>
                    <Paper className={this.props.classes.video}>
                        <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=O6HnfsjFQAg"/>
                        <Typography variant="title" className={this.props.classes.caseStudy}>How To: Use The Hex Viewer</Typography>
                    </Paper>
                    <Paper className={this.props.classes.video}>
                        <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=O6HnfsjFQAg"/>
                        <Typography variant="title" className={this.props.classes.caseStudy}>How To: Use The Registry View</Typography>
                    </Paper>
                    <Paper className={this.props.classes.video}>
                        <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=O6HnfsjFQAg"/>
                        <Typography variant="title" className={this.props.classes.caseStudy}>How To: Filter Shellbags</Typography>
                    </Paper>
                    <Paper className={this.props.classes.video}>
                        <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=O6HnfsjFQAg"/>
                        <Typography variant="title" className={this.props.classes.caseStudy}>How To: Export A Report</Typography>
                    </Paper>
                    <Paper className={this.props.classes.video}>
                        <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=O6HnfsjFQAg"/>
                        <Typography variant="title" className={this.props.classes.caseStudy}>How To: Export A Report</Typography>
                    </Paper>
                    <Paper className={this.props.classes.video}>
                        <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=O6HnfsjFQAg"/>
                        <Typography variant="title" className={this.props.classes.caseStudy}>How To: Export A Report</Typography>
                    </Paper>
                    <Paper className={this.props.classes.video}>
                        <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=O6HnfsjFQAg"/>
                        <Typography variant="title" className={this.props.classes.caseStudy}>How To: Export A Report</Typography>
                    </Paper>
                    <Paper className={this.props.classes.video}>
                        <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=O6HnfsjFQAg"/>
                        <Typography variant="title" className={this.props.classes.caseStudy}>How To: Export A Report</Typography>
                    </Paper>
                    <Paper className={this.props.classes.video}>
                        <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=O6HnfsjFQAg"/>
                        <Typography variant="title" className={this.props.classes.caseStudy}>How To: Export A Report</Typography>
                    </Paper>
                    <Paper className={this.props.classes.video}>
                        <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=O6HnfsjFQAg"/>
                        <Typography variant="title" className={this.props.classes.caseStudy}>How To: Export A Report</Typography>
                    </Paper>
                </Paper>
            </div>
        )
    }
}

export default withStyles(styles)(CaseStudies);