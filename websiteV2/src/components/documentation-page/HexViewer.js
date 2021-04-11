import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import Typography from '@material-ui/core/Typography';
import HexViewerImg from '../../assets/hex-viewer.png';
import HexHighlight from '../../assets/hex-viewer-highlight.png';

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
}

class HexViewer extends React.Component {

    render() {
        return (
            <div className={this.props.classes.content}>
                <Typography variant="title" className={this.props.classes.title}>Hex Viewer</Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    The hex viewer displays shellbag data in its most raw form, directly taken out of the Windows Registry without extracting fields from it. 
                    If shellbag definitions are incomplete, or information is missing in the parser, the hex viewer can provide insight.
                </Typography>
                <img src={HexViewerImg} alt="hex-viewer" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    Each hex value (or set of hex values) corresponds to a shellbag field. For instance, the first two values in the first row correspond to
                    the size of the shell, meanwhile the third value in the first row corresponds to the type field.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    Hex values can be highlighted to look at only a particular section of interest.
                </Typography>
                <img src={HexHighlight} alt="highlighted-hex-viewer" />
            </div>
        )
    }
}

export default withStyles(styles)(HexViewer);