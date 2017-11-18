package me.alistairmclean.cc01;
import android.util.Log;

import java.io.DataOutputStream;
import java.io.IOException;
import java.net.Socket;
import java.net.UnknownHostException;

/**
 * Written by David Singleton.
 * File recreated on 11/3/2017.
 */

public class FeatureStreamer {
    private String mHOST_IP;
    private int mHOST_PORT;
    private Socket mSocket;
    private DataOutputStream mDOS;
    private boolean mIsConnected = false;


    FeatureStreamer() {
    }

    void connect(String addr, int port) {
        Log.d("FEATURE STREAMER: ", "add = " + addr + " port = " + port);
        try {
            mSocket = new Socket(addr, port);
            mDOS = new DataOutputStream(mSocket.getOutputStream());
            mIsConnected = true;
        } catch (UnknownHostException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    void sendByteArray(byte[] send) throws IOException {
        if (mDOS != null) {
            mDOS.writeInt(send.length);
            mDOS.write(send, 0, send.length);
            mDOS.flush();
        }
    }
    boolean isConnected() {
        return mIsConnected;
    }

    void sendFeatures(int width, int height, byte[] send) {
        try {
            if (mDOS != null) {
                mDOS.writeInt(width);
                mDOS.writeInt(height);
                mDOS.write(send, 0, send.length);
                mDOS.flush();
            }
        } catch (IOException e) {
        }
    }

    void close() throws IOException {
        if (mDOS != null) {
            mDOS.close();
        }
        if (mSocket != null) {
            mSocket.close();
        }
        mIsConnected = false;
    }


}
